namespace MiniCrm.Application.DTOs.Auth;

public class AuthResponseDto
{
    public Guid UserId { get; set; }

    public string Email { get; set; } = string.Empty;

    public List<string> Roles { get; set; } = new();

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; set; }
}