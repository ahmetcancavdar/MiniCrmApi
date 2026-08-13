namespace MiniCrm.Application.DTOs.Auth;

public class TokenResultDto
{
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; set; }
}