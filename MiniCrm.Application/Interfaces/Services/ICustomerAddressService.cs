using MiniCrm.Application.DTOs.Addresses;

namespace MiniCrm.Application.Interfaces.Services;

public interface ICustomerAddressService
{
    Task<List<AddressResponseDto>> GetAllAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<AddressResponseDto> CreateAsync(
        Guid userId,
        CreateAddressRequestDto request,
        CancellationToken cancellationToken = default);

    Task<AddressResponseDto> UpdateAsync(
        Guid userId,
        int addressId,
        UpdateAddressRequestDto request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid userId,
        int addressId,
        CancellationToken cancellationToken = default);

    Task<AddressResponseDto> SetDefaultAsync(
        Guid userId,
        int addressId,
        CancellationToken cancellationToken = default);
}