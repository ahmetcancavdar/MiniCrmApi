using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using MiniCrm.Application.Common;
using MiniCrm.Application.DTOs.Leads;
using MiniCrm.Application.Interfaces;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Application.Interfaces.Services;
using MiniCrm.Domain.Entities;
using MiniCrm.Domain.Enums;
using MiniCrm.Persistence.Context;
using MiniCrm.Persistence.Identity;

namespace MiniCrm.Persistence.Services;

public sealed class LeadService : ILeadService
{
    private readonly ILeadRepository _leadRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IEmailLogRepository _emailLogRepository;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _context;

    public LeadService(
        ILeadRepository leadRepository,
        ICustomerRepository customerRepository,
        ICartRepository cartRepository,
        IEmailLogRepository emailLogRepository,
        IEmailService emailService,
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager,
        AppDbContext context)
    {
        _leadRepository = leadRepository;
        _customerRepository = customerRepository;
        _cartRepository = cartRepository;
        _emailLogRepository = emailLogRepository;
        _emailService = emailService;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _context = context;
    }


    // ============================================================
    // LIST
    // ============================================================

    public async Task<List<LeadResponseDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var leads =
            await _leadRepository.GetAllAsync(
                cancellationToken);

        return leads
            .Select(Map)
            .ToList();
    }


    // ============================================================
    // DETAIL
    // ============================================================

    public async Task<LeadDetailResponseDto> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var lead =
            await _leadRepository.GetWithDetailsAsync(
                id,
                cancellationToken)
            ?? throw new KeyNotFoundException(
                "Lead was not found.");

        return MapDetail(lead);
    }


    // ============================================================
    // CREATE
    // ============================================================

    public async Task<LeadResponseDto> CreateAsync(
        CreateLeadRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var email = request.Email
            .Trim()
            .ToLowerInvariant();

        await EnsureEmailIsAvailableAsync(
            email,
            excludeLeadId: null,
            cancellationToken);

        var lead =
            new Lead(
                request.FullName,
                request.CompanyName,
                email,
                request.Phone,
                request.Source,
                request.InterestArea,
                request.Notes,
                request.NextFollowUpDate);

        await _leadRepository.AddAsync(
            lead,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Map(lead);
    }


    // ============================================================
    // UPDATE
    // ============================================================

    public async Task<LeadResponseDto> UpdateAsync(
        int id,
        UpdateLeadRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var lead =
            await _leadRepository.GetByIdAsync(
                id,
                cancellationToken)
            ?? throw new KeyNotFoundException(
                "Lead was not found.");

        var email = request.Email
            .Trim()
            .ToLowerInvariant();

        if (email != lead.Email)
        {
            await EnsureEmailIsAvailableAsync(
                email,
                excludeLeadId: id,
                cancellationToken);
        }

        lead.Update(
            request.FullName,
            request.CompanyName,
            email,
            request.Phone,
            request.Source,
            request.InterestArea,
            request.Notes,
            request.NextFollowUpDate);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Map(lead);
    }


    // ============================================================
    // UPDATE STATUS
    // ============================================================

    public async Task<LeadResponseDto> UpdateStatusAsync(
        int id,
        Guid adminUserId,
        UpdateLeadStatusRequestDto request,
        CancellationToken cancellationToken = default)
    {
        // Notlar listesinde otomatik bir kayıt oluşturulacağı için (bkz.
        // aşağıdaki AddNote çağrısı), Lead ile birlikte mevcut notlar da
        // yüklenmeli; aksi halde EF Core henüz materyalize edilmemiş bir
        // koleksiyona yapılan ekleme SaveChanges sırasında takip edilmeyebilir.
        var lead =
            await _leadRepository.GetWithDetailsAsync(
                id,
                cancellationToken)
            ?? throw new KeyNotFoundException(
                "Lead was not found.");

        var previousStatus =
            lead.Status;

        switch (request.Status)
        {
            case LeadStatus.New:

                throw new InvalidOperationException(
                    "A lead cannot be moved back to New.");

            case LeadStatus.Contacted:

                lead.MarkAsContacted();
                break;

            case LeadStatus.Qualified:

                lead.MarkAsQualified();
                break;

            case LeadStatus.ProposalSent:

                lead.MarkProposalSent();
                break;

            case LeadStatus.Lost:

                lead.MarkAsLost(request.Reason);
                break;

            case LeadStatus.Converted:

                throw new InvalidOperationException(
                    "Use the convert endpoint to convert a lead into a customer.");

            default:

                throw new InvalidOperationException(
                    "Unknown lead status.");
        }

        var statusChangeNote =
            $"Durum '{GetStatusDisplayName(previousStatus)}' konumundan '{GetStatusDisplayName(lead.Status)}' konumuna güncellendi.";

        if (!string.IsNullOrWhiteSpace(request.Reason))
        {
            statusChangeNote +=
                $" Neden: {request.Reason.Trim()}";
        }

        lead.AddNote(
            adminUserId,
            statusChangeNote);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Map(lead);
    }


    // ============================================================
    // ADD NOTE
    // ============================================================

    public async Task<LeadDetailResponseDto> AddNoteAsync(
        int id,
        Guid adminUserId,
        AddLeadNoteRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var lead =
            await _leadRepository.GetWithDetailsAsync(
                id,
                cancellationToken)
            ?? throw new KeyNotFoundException(
                "Lead was not found.");

        lead.AddNote(
            adminUserId,
            request.Note);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapDetail(lead);
    }


    // ============================================================
    // CONVERT TO CUSTOMER
    // ============================================================

    public async Task<LeadResponseDto> ConvertToCustomerAsync(
        int id,
        ConvertLeadRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var lead =
            await _leadRepository.GetByIdAsync(
                id,
                cancellationToken)
            ?? throw new KeyNotFoundException(
                "Lead was not found.");

        if (lead.Status == LeadStatus.Converted)
        {
            throw new InvalidOperationException(
                "This lead has already been converted.");
        }


        // ========================================================
        // Aynı email ile bir Customer zaten varsa yeni hesap
        // açmadan doğrudan o Customer'a bağlanır.
        // ========================================================

        var existingCustomer =
            await _customerRepository.GetByEmailAsync(
                lead.Email,
                cancellationToken);

        if (existingCustomer is not null)
        {
            lead.ConvertToCustomer(
                existingCustomer.Id);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return Map(lead);
        }


        // ========================================================
        // Yeni bir ApplicationUser + Customer + Cart oluştur.
        // ========================================================

        await using var transaction =
            await _context.Database.BeginTransactionAsync(
                cancellationToken);

        Customer customer;
        string temporaryPassword;

        try
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),

                UserName = lead.Email,

                Email = lead.Email,

                PhoneNumber = lead.Phone,

                IsActive = true,

                LockoutEnabled = true
            };

            temporaryPassword =
                GenerateTemporaryPassword();

            var createResult =
                await _userManager.CreateAsync(
                    user,
                    temporaryPassword);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(
                    " | ",
                    createResult.Errors
                        .Select(x => x.Description));

                throw new InvalidOperationException(
                    errors);
            }

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

            customer = new Customer
            {
                UserId = user.Id,

                FullName = lead.FullName,

                Email = lead.Email,

                Phone = lead.Phone,

                CompanyName = lead.CompanyName
            };

            await _customerRepository.AddAsync(
                customer,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            var cart =
                new Cart(customer.Id);

            await _cartRepository.AddAsync(
                cart,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            await transaction.CommitAsync(
                cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(
                cancellationToken);

            throw;
        }


        // ========================================================
        // Bildirim e-postası (başarısız olsa bile dönüşümü
        // engellemez, sadece loglanır).
        // ========================================================

        await SendAccountCreatedEmailAsync(
            customer,
            temporaryPassword,
            cancellationToken);

        lead.ConvertToCustomer(
            customer.Id);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Map(lead);
    }


    // ============================================================
    // DELETE
    // ============================================================

    public async Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var lead =
            await _leadRepository.GetByIdAsync(
                id,
                cancellationToken)
            ?? throw new KeyNotFoundException(
                "Lead was not found.");

        lead.SoftDelete();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ============================================================
    // HELPERS
    // ============================================================

    private static string GetStatusDisplayName(
        LeadStatus status)
    {
        return status switch
        {
            LeadStatus.New => "Yeni",
            LeadStatus.Contacted => "İletişime Geçildi",
            LeadStatus.Qualified => "Değerlendirildi",
            LeadStatus.ProposalSent => "Teklif Gönderildi",
            LeadStatus.Converted => "Dönüştürüldü",
            LeadStatus.Lost => "Kaybedildi",
            _ => status.ToString()
        };
    }

    private async Task EnsureEmailIsAvailableAsync(
        string email,
        int? excludeLeadId,
        CancellationToken cancellationToken)
    {
        var activeLeadExists =
            await _leadRepository.ExistsActiveByEmailAsync(
                email,
                excludeLeadId,
                cancellationToken);

        if (activeLeadExists)
        {
            throw new InvalidOperationException(
                "An active lead with this email already exists.");
        }

        var customerExists =
            await _customerRepository.GetByEmailAsync(
                email,
                cancellationToken)
            is not null;

        if (customerExists)
        {
            throw new InvalidOperationException(
                "A customer with this email already exists.");
        }
    }

    private async Task SendAccountCreatedEmailAsync(
        Customer customer,
        string temporaryPassword,
        CancellationToken cancellationToken)
    {
        var subject =
            "MiniCrm - Hesabınız Oluşturuldu";

        var body =
            $"""
            Merhaba {customer.FullName},

            MiniCrm sisteminde sizin için bir müşteri hesabı oluşturuldu.

            E-posta: {customer.Email}
            Geçici Şifre: {temporaryPassword}

            Giriş yaptıktan sonra şifrenizi değiştirmenizi öneririz.
            """;

        var emailLog =
            new EmailLog(
                customer.Email,
                subject,
                body,
                EmailType.LeadConverted,
                customer.Id);

        await _emailLogRepository.AddAsync(
            emailLog,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        try
        {
            await _emailService.SendAsync(
                customer.Email,
                subject,
                body,
                cancellationToken);

            emailLog.MarkAsSent(
                DateTime.UtcNow);
        }
        catch (Exception exception)
        {
            emailLog.MarkAsFailed(
                exception.Message,
                DateTime.UtcNow);
        }

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }

    private static string GenerateTemporaryPassword()
    {
        var random =
            RandomNumberGenerator.GetHexString(12);

        return $"Mn{random}7!";
    }

    private static LeadResponseDto Map(
        Lead lead)
    {
        return new LeadResponseDto
        {
            Id = lead.Id,
            FullName = lead.FullName,
            CompanyName = lead.CompanyName,
            Email = lead.Email,
            Phone = lead.Phone,
            Source = lead.Source.ToString(),
            Status = lead.Status.ToString(),
            InterestArea = lead.InterestArea,
            Notes = lead.Notes,
            NextFollowUpDate = lead.NextFollowUpDate,
            ConvertedCustomerId = lead.ConvertedCustomerId,
            CreatedAtUtc = lead.CreatedAtUtc
        };
    }

    private static LeadDetailResponseDto MapDetail(
        Lead lead)
    {
        return new LeadDetailResponseDto
        {
            Id = lead.Id,
            FullName = lead.FullName,
            CompanyName = lead.CompanyName,
            Email = lead.Email,
            Phone = lead.Phone,
            Source = lead.Source.ToString(),
            Status = lead.Status.ToString(),
            InterestArea = lead.InterestArea,
            Notes = lead.Notes,
            NextFollowUpDate = lead.NextFollowUpDate,
            ConvertedCustomerId = lead.ConvertedCustomerId,
            CreatedAtUtc = lead.CreatedAtUtc,
            LeadNotes = lead.LeadNotes
                .OrderByDescending(x => x.CreatedAtUtc)
                .Select(x => new LeadNoteResponseDto
                {
                    Id = x.Id,
                    AdminUserId = x.AdminUserId,
                    Note = x.Note,
                    CreatedAtUtc = x.CreatedAtUtc
                })
                .ToList()
        };
    }
}
