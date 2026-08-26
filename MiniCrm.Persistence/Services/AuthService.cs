using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniCrm.Application.Common;
using MiniCrm.Application.DTOs.Auth;
using MiniCrm.Application.Interfaces;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Application.Interfaces.Services;
using MiniCrm.Domain.Entities;
using MiniCrm.Persistence.Context;
using MiniCrm.Persistence.Identity;

namespace MiniCrm.Persistence.Services;

public sealed class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly AppDbContext _context;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        ICustomerRepository customerRepository,
        ICartRepository cartRepository,
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        AppDbContext context)
    {
        _userManager = userManager;
        _customerRepository = customerRepository;
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _context = context;
    }


    // ============================================================
    // REGISTER
    // ============================================================

    public async Task<AuthResponseDto> RegisterAsync(
        RegisterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var email = request.Email
            .Trim()
            .ToLowerInvariant();

        // Aynı email ile daha önce kullanıcı oluşturulmuş mu?
        var existingUser =
            await _userManager.FindByEmailAsync(email);

        if (existingUser is not null)
        {
            throw new InvalidOperationException(
                "An account with this email already exists.");
        }


        // ========================================================
        // TRANSACTION BAŞLAT
        // ========================================================

        await using var transaction =
            await _context.Database.BeginTransactionAsync(
                cancellationToken);

        try
        {
            // ====================================================
            // 1. IDENTITY USER OLUŞTUR
            // ====================================================

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),

                UserName = email,

                Email = email,

                PhoneNumber =
                    request.Phone?.Trim(),

                IsActive = true,

                LockoutEnabled = true
            };


            // Password hash işlemini Identity kendisi yapar.
            var createResult =
                await _userManager.CreateAsync(
                    user,
                    request.Password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(
                    " | ",
                    createResult.Errors
                        .Select(x => x.Description));

                throw new InvalidOperationException(
                    errors);
            }


            // ====================================================
            // 2. CUSTOMER ROLE ATA
            // ====================================================

            var roleResult =
                await _userManager.AddToRoleAsync(
                    user,
                    AppRoles.Customer);

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(
                    " | ",
                    roleResult.Errors
                        .Select(x => x.Description));

                throw new InvalidOperationException(
                    errors);
            }


            // ====================================================
            // 3. CUSTOMER PROFİLİ OLUŞTUR
            // ====================================================

            var customer = new Customer
            {
                UserId = user.Id,

                FullName =
                    request.FullName.Trim(),

                Email = email,

                Phone =
                    request.Phone?.Trim(),

                CompanyName =
                    request.CompanyName?.Trim()
            };


            await _customerRepository.AddAsync(
                customer,
                cancellationToken);


            // Customer'ın Id değerini alabilmek için
            // önce database'e kaydediyoruz.
            await _unitOfWork.SaveChangesAsync(
                cancellationToken);


            // ====================================================
            // 4. CUSTOMER'A CART OLUŞTUR
            // ====================================================

            var cart =
                new Cart(customer.Id);


            await _cartRepository.AddAsync(
                cart,
                cancellationToken);


            await _unitOfWork.SaveChangesAsync(
                cancellationToken);


            // ====================================================
            // 5. TRANSACTION COMMIT
            // ====================================================

            await transaction.CommitAsync(
                cancellationToken);


            // ====================================================
            // 6. USER ROLE'LERİNİ GETİR
            // ====================================================

            var roles =
                await _userManager.GetRolesAsync(user);


            // ====================================================
            // 7. JWT + AUTH RESPONSE
            // ====================================================

            return CreateAuthResponse(
                user,
                roles.ToList());
        }
        catch
        {
            await transaction.RollbackAsync(
                cancellationToken);

            throw;
        }
    }


    // ============================================================
    // LOGIN
    // ============================================================

    public async Task<AuthResponseDto> LoginAsync(
        LoginRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var email = request.Email
            .Trim()
            .ToLowerInvariant();


        // ========================================================
        // 1. KULLANICIYI BUL
        // ========================================================

        var user =
            await _userManager.FindByEmailAsync(email);


        // Kullanıcı bulunamadıysa veya pasifse
        if (user is null || !user.IsActive)
        {
            throw new UnauthorizedAccessException(
                "Email or password is incorrect.");
        }


        // ========================================================
        // 2. ACCOUNT LOCKOUT KONTROLÜ
        // ========================================================

        if (await _userManager.IsLockedOutAsync(user))
        {
            throw new UnauthorizedAccessException(
                "The account is temporarily locked.");
        }


        // ========================================================
        // 3. PASSWORD KONTROLÜ
        // ========================================================

        var passwordValid =
            await _userManager.CheckPasswordAsync(
                user,
                request.Password);


        if (!passwordValid)
        {
            // Başarısız giriş sayısını artır.
            await _userManager.AccessFailedAsync(user);

            throw new UnauthorizedAccessException(
                "Email or password is incorrect.");
        }


        // ========================================================
        // 4. BAŞARILI LOGIN
        // ========================================================

        // Başarısız login sayısını sıfırla.
        await _userManager.ResetAccessFailedCountAsync(
            user);


        // Kullanıcının rollerini getir.
        var roles =
            await _userManager.GetRolesAsync(user);


        // ========================================================
        // 5. JWT + RESPONSE
        // ========================================================

        return CreateAuthResponse(
            user,
            roles.ToList());
    }


    // ============================================================
    // CHANGE PASSWORD
    // ============================================================

    public async Task ChangePasswordAsync(
        Guid userId,
        ChangePasswordRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var user =
            await _userManager.FindByIdAsync(
                userId.ToString())
            ?? throw new UnauthorizedAccessException(
                "User not found.");

        if (await _userManager.IsLockedOutAsync(user))
        {
            throw new UnauthorizedAccessException(
                "The account is temporarily locked.");
        }

        // Mevcut şifre burada ayrıca kontrol edilir ki yanlış denemeler
        // (LoginAsync'teki gibi) lockout sayacına dahil olsun; aksi halde
        // Identity'nin ChangePasswordAsync'i mevcut şifreyi kendi içinde
        // doğruluyor olsa da başarısız denemeler hiç sayılmaz ve bu
        // endpoint sınırsız brute-force denemesine açık kalırdı.
        var currentPasswordValid =
            await _userManager.CheckPasswordAsync(
                user,
                request.CurrentPassword);

        if (!currentPasswordValid)
        {
            await _userManager.AccessFailedAsync(
                user);

            throw new UnauthorizedAccessException(
                "Current password is incorrect.");
        }

        var result =
            await _userManager.ChangePasswordAsync(
                user,
                request.CurrentPassword,
                request.NewPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                " | ",
                result.Errors
                    .Select(x => x.Description));

            throw new InvalidOperationException(
                errors);
        }

        await _userManager.ResetAccessFailedCountAsync(
            user);
    }


    // ============================================================
    // CHANGE EMAIL
    // ============================================================

    public async Task ChangeEmailAsync(
        Guid userId,
        ChangeEmailRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var user =
            await _userManager.FindByIdAsync(
                userId.ToString())
            ?? throw new UnauthorizedAccessException(
                "User not found.");

        if (await _userManager.IsLockedOutAsync(user))
        {
            throw new UnauthorizedAccessException(
                "The account is temporarily locked.");
        }

        var passwordValid =
            await _userManager.CheckPasswordAsync(
                user,
                request.CurrentPassword);

        if (!passwordValid)
        {
            // LoginAsync ile aynı şekilde başarısız denemeyi lockout
            // sayacına dahil et (bkz. ChangePasswordAsync'teki not).
            await _userManager.AccessFailedAsync(
                user);

            throw new UnauthorizedAccessException(
                "Current password is incorrect.");
        }

        await _userManager.ResetAccessFailedCountAsync(
            user);

        var newEmail = request.NewEmail
            .Trim()
            .ToLowerInvariant();

        var existingUser =
            await _userManager.FindByEmailAsync(
                newEmail);

        if (existingUser is not null &&
            existingUser.Id != user.Id)
        {
            throw new InvalidOperationException(
                "An account with this email already exists.");
        }

        var setEmailResult =
            await _userManager.SetEmailAsync(
                user,
                newEmail);

        if (!setEmailResult.Succeeded)
        {
            var errors = string.Join(
                " | ",
                setEmailResult.Errors
                    .Select(x => x.Description));

            throw new InvalidOperationException(
                errors);
        }

        var setUserNameResult =
            await _userManager.SetUserNameAsync(
                user,
                newEmail);

        if (!setUserNameResult.Succeeded)
        {
            var errors = string.Join(
                " | ",
                setUserNameResult.Errors
                    .Select(x => x.Description));

            throw new InvalidOperationException(
                errors);
        }

        var customer =
            await _customerRepository.GetByUserIdAsync(
                userId,
                cancellationToken);

        if (customer is not null)
        {
            customer.Email =
                newEmail;

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);
        }
    }


    // ============================================================
    // AUTH RESPONSE OLUŞTUR
    // ============================================================

    private AuthResponseDto CreateAuthResponse(
        ApplicationUser user,
        IReadOnlyCollection<string> roles)
    {
        var email =
            user.Email
            ?? throw new InvalidOperationException(
                "User email is missing.");


        // JWT oluştur.
        var tokenResult =
            _tokenService.CreateToken(
                user.Id,
                email,
                roles);


        return new AuthResponseDto
        {
            UserId = user.Id,

            Email = email,

            Roles = roles.ToList(),

            Token = tokenResult.Token,

            ExpiresAtUtc =
                tokenResult.ExpiresAtUtc
        };
    }
}