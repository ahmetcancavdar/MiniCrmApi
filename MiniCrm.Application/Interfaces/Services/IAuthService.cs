using MiniCrm.Application.DTOs.Auth;

namespace MiniCrm.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(
        RegisterRequestDto request,
        CancellationToken cancellationToken = default);

    Task<AuthResponseDto> LoginAsync(
        LoginRequestDto request,
        CancellationToken cancellationToken = default);

    Task ChangePasswordAsync(
        Guid userId,
        ChangePasswordRequestDto request,
        CancellationToken cancellationToken = default);

    Task ChangeEmailAsync(
        Guid userId,
        ChangeEmailRequestDto request,
        CancellationToken cancellationToken = default);
}