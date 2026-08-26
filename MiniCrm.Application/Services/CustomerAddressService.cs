using MiniCrm.Application.DTOs.Addresses;
using MiniCrm.Application.Interfaces;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Application.Interfaces.Services;
using MiniCrm.Domain.Entities;

namespace MiniCrm.Application.Services;

public sealed class CustomerAddressService
    : ICustomerAddressService
{
    private readonly ICustomerRepository
        _customerRepository;

    private readonly ICustomerAddressRepository
        _addressRepository;

    private readonly IUnitOfWork
        _unitOfWork;

    public CustomerAddressService(
        ICustomerRepository customerRepository,
        ICustomerAddressRepository addressRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository =
            customerRepository;

        _addressRepository =
            addressRepository;

        _unitOfWork =
            unitOfWork;
    }


    // ============================================================
    // GET ALL
    // ============================================================

    public async Task<List<AddressResponseDto>>
        GetAllAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        var addresses =
            await _addressRepository
                .GetByCustomerIdAsync(
                    customer.Id,
                    cancellationToken);

        return addresses
            .Select(Map)
            .ToList();
    }


    // ============================================================
    // CREATE
    // ============================================================

    public async Task<AddressResponseDto> CreateAsync(
        Guid userId,
        CreateAddressRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        var existingAddresses =
            await _addressRepository
                .GetByCustomerIdAsync(
                    customer.Id,
                    cancellationToken);

        var shouldBeDefault =
            existingAddresses.Count == 0 ||
            request.IsDefault;

        if (shouldBeDefault)
        {
            foreach (var existingAddress
                     in existingAddresses)
            {
                existingAddress.IsDefault =
                    false;
            }
        }

        var address =
            new CustomerAddress
            {
                CustomerId =
                    customer.Id,

                Title =
                    request.Title.Trim(),

                AddressLine =
                    request.AddressLine.Trim(),

                City =
                    request.City.Trim(),

                District =
                    request.District.Trim(),

                PostalCode =
                    NormalizeOptional(
                        request.PostalCode)
                    ?? string.Empty,

                Country =
                    request.Country.Trim(),

                IsDefault =
                    shouldBeDefault
            };

        await _addressRepository.AddAsync(
            address,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Map(address);
    }


    // ============================================================
    // UPDATE
    // ============================================================

    public async Task<AddressResponseDto> UpdateAsync(
        Guid userId,
        int addressId,
        UpdateAddressRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        var address =
            await GetOwnedAddressAsync(
                customer.Id,
                addressId,
                cancellationToken);

        address.Title =
            request.Title.Trim();

        address.AddressLine =
            request.AddressLine.Trim();

        address.City =
            request.City.Trim();

        address.District =
            request.District.Trim();

        address.PostalCode =
            NormalizeOptional(
                request.PostalCode)
            ?? string.Empty;

        address.Country =
            request.Country.Trim();

        if (request.IsDefault &&
            !address.IsDefault)
        {
            var otherAddresses =
                await _addressRepository
                    .GetByCustomerIdAsync(
                        customer.Id,
                        cancellationToken);

            foreach (var otherAddress in otherAddresses
                     .Where(x => x.Id != address.Id))
            {
                otherAddress.IsDefault =
                    false;
            }

            address.IsDefault =
                true;
        }
        else if (!request.IsDefault &&
                 address.IsDefault)
        {
            address.IsDefault =
                false;
        }

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Map(address);
    }


    // ============================================================
    // DELETE
    // ============================================================

    public async Task DeleteAsync(
        Guid userId,
        int addressId,
        CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        var address =
            await GetOwnedAddressAsync(
                customer.Id,
                addressId,
                cancellationToken);

        var wasDefault =
            address.IsDefault;

        var addresses =
            await _addressRepository
                .GetByCustomerIdAsync(
                    customer.Id,
                    cancellationToken);

        var remainingAddresses =
            addresses
                .Where(x =>
                    x.Id != address.Id)
                .ToList();

        if (wasDefault &&
            remainingAddresses.Count > 0)
        {
            var nextDefault =
                remainingAddresses
                    .OrderBy(x => x.Id)
                    .First();

            nextDefault.IsDefault =
                true;
        }

        _addressRepository.Remove(
            address);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }


    // ============================================================
    // SET DEFAULT
    // ============================================================

    public async Task<AddressResponseDto>
        SetDefaultAsync(
            Guid userId,
            int addressId,
            CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        var targetAddress =
            await GetOwnedAddressAsync(
                customer.Id,
                addressId,
                cancellationToken);

        var addresses =
            await _addressRepository
                .GetByCustomerIdAsync(
                    customer.Id,
                    cancellationToken);

        foreach (var address in addresses)
        {
            address.IsDefault =
                address.Id ==
                targetAddress.Id;
        }

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Map(targetAddress);
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
    // OWNERSHIP
    // ============================================================

    private async Task<CustomerAddress>
        GetOwnedAddressAsync(
            int customerId,
            int addressId,
            CancellationToken cancellationToken)
    {
        var address =
            await _addressRepository
                .GetByIdAsync(
                    addressId,
                    cancellationToken)
            ?? throw new KeyNotFoundException(
                "Address was not found.");

        if (address.CustomerId !=
            customerId)
        {
            throw new UnauthorizedAccessException(
                "You do not have access to this address.");
        }

        return address;
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

    private static AddressResponseDto Map(
        CustomerAddress address)
    {
        return new AddressResponseDto
        {
            Id =
                address.Id,

            Title =
                address.Title,

            AddressLine =
                address.AddressLine,

            City =
                address.City,

            District =
                address.District,

            PostalCode =
                string.IsNullOrWhiteSpace(
                    address.PostalCode)
                    ? null
                    : address.PostalCode,

            Country =
                address.Country,

            IsDefault =
                address.IsDefault,

            CreatedAtUtc =
                address.CreatedAtUtc
        };
    }
}