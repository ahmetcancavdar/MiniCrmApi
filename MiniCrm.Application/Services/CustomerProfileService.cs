using MiniCrm.Application.DTOs.Profile;
using MiniCrm.Application.Interfaces;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Application.Interfaces.Services;
using MiniCrm.Domain.Entities;

namespace MiniCrm.Application.Services;

public sealed class CustomerProfileService
    : ICustomerProfileService
{
    private readonly ICustomerRepository
        _customerRepository;

    private readonly IUnitOfWork
        _unitOfWork;

    public CustomerProfileService(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository =
            customerRepository;

        _unitOfWork =
            unitOfWork;
    }


    // ============================================================
    // GET PROFILE
    // ============================================================

    public async Task<ProfileResponseDto> GetAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        return Map(customer);
    }


    // ============================================================
    // UPDATE PROFILE
    // ============================================================

    public async Task<ProfileResponseDto> UpdateAsync(
        Guid userId,
        UpdateProfileRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        customer.FullName =
            request.FullName.Trim();

        customer.Phone =
            NormalizeOptional(
                request.Phone);

        customer.CompanyName =
            NormalizeOptional(
                request.CompanyName);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Map(customer);
    }


    // ============================================================
    // ADMIN - LIST ALL
    // ============================================================

    public async Task<List<ProfileResponseDto>> GetAllForAdminAsync(
        CancellationToken cancellationToken = default)
    {
        var customers =
            await _customerRepository.GetAllAsync(
                cancellationToken);

        return customers
            .Select(Map)
            .ToList();
    }


    // ============================================================
    // CUSTOMER
    // ============================================================

    private async Task<Customer> GetCustomerAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException(
                "Invalid user.");
        }

        return await _customerRepository
            .GetByUserIdAsync(
                userId,
                cancellationToken)
            ?? throw new UnauthorizedAccessException(
                "Customer profile was not found.");
    }


    // ============================================================
    // HELPERS
    // ============================================================

    private static string? NormalizeOptional(
        string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }


    // ============================================================
    // MAP
    // ============================================================

    private static ProfileResponseDto Map(
        Customer customer)
    {
        return new ProfileResponseDto
        {
            CustomerId =
                customer.Id,

            UserId =
                customer.UserId,

            FullName =
                customer.FullName,

            Email =
                customer.Email,

            Phone =
                customer.Phone,

            CompanyName =
                customer.CompanyName,

            CreatedAtUtc =
                customer.CreatedAtUtc
        };
    }
}