using MiniCrm.Application.DTOs.Auth;

namespace MiniCrm.Application.Interfaces.Services;

public interface ITokenService
{
    TokenResultDto CreateToken(
        Guid userId,
        string email,
        IReadOnlyCollection<string> roles);
}