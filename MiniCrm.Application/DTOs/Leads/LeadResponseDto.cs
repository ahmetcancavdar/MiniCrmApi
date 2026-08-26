namespace MiniCrm.Application.DTOs.Leads;

public class LeadResponseDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string? CompanyName { get; set; }

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string Source { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string? InterestArea { get; set; }

    public string? Notes { get; set; }

    public DateTime? NextFollowUpDate { get; set; }

    public int? ConvertedCustomerId { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}
