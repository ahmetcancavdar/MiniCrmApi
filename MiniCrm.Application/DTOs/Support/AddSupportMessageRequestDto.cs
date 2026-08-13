using System.ComponentModel.DataAnnotations;

namespace MiniCrm.Application.DTOs.Support;

public class AddSupportMessageRequestDto
{
    [Required]
    [MaxLength(4000)]
    public string Message { get; set; } =
        string.Empty;
}