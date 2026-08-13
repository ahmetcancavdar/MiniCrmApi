namespace MiniCrm.Application.DTOs.Profile;

public class ProfileResponseDto
{
    public int CustomerId { get; set; }

    public Guid UserId { get; set; }

    public string FullName { get; set; } =
        string.Empty;

    public string Email { get; set; } =
        string.Empty;

    public string? Phone { get; set; }

    public string? CompanyName { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}