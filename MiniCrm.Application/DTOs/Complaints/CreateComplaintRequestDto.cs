using System.ComponentModel.DataAnnotations;

namespace MiniCrm.Application.DTOs.Complaints;

public class CreateComplaintRequestDto
{
    [Required]
    [MaxLength(200)]
    public string Subject { get; set; } =
        string.Empty;

    [Required]
    [MaxLength(4000)]
    public string Description { get; set; } =
        string.Empty;

    public int? OrderId { get; set; }
}