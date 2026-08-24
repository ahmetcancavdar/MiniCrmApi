using MiniCrm.Application.DTOs.Profile;

namespace MiniCrm.Application.Interfaces.Services;

public interface ICustomerProfileService
{
    Task<ProfileResponseDto> GetAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<ProfileResponseDto> UpdateAsync(
        Guid userId,
        UpdateProfileRequestDto request,
        CancellationToken cancellationToken = default);

    Task<List<ProfileResponseDto>> GetAllForAdminAsync(
        CancellationToken cancellationToken = default);
}