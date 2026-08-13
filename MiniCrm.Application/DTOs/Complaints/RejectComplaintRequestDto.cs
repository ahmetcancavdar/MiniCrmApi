using System.ComponentModel.DataAnnotations;

namespace MiniCrm.Application.DTOs.Complaints;

public class RejectComplaintRequestDto
{
    [Required]
    [MaxLength(2000)]
    public string AdminNote { get; set; } =
        string.Empty;
}