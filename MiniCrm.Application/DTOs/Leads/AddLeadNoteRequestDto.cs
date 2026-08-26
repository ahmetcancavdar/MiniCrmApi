using System.ComponentModel.DataAnnotations;

namespace MiniCrm.Application.DTOs.Leads;

public class AddLeadNoteRequestDto
{
    [Required]
    [MaxLength(2000)]
    public string Note { get; set; } = string.Empty;
}
