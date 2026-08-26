using System.ComponentModel.DataAnnotations;

namespace MiniCrm.Application.DTOs.Auth;

public class ChangeEmailRequestDto
{
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string NewEmail { get; set; } = string.Empty;

    [Required]
    public string CurrentPassword { get; set; } = string.Empty;
}
