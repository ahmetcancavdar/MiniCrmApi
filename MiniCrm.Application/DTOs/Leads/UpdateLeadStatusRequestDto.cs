using System.ComponentModel.DataAnnotations;
using MiniCrm.Domain.Enums;

namespace MiniCrm.Application.DTOs.Leads;

public class UpdateLeadStatusRequestDto
{
    public LeadStatus Status { get; set; }

    [MaxLength(500)]
    public string? Reason { get; set; }
}
