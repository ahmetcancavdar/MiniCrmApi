using System.ComponentModel.DataAnnotations;
using MiniCrm.Domain.Enums;

namespace MiniCrm.Application.DTOs.Leads;

public class CreateLeadRequestDto
{
    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? CompanyName { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? Phone { get; set; }

    public LeadSource Source { get; set; }

    [MaxLength(250)]
    public string? InterestArea { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public DateTime? NextFollowUpDate { get; set; }
}
