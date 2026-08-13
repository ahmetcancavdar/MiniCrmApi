using System.ComponentModel.DataAnnotations;

namespace MiniCrm.Application.DTOs.Profile;

public class UpdateProfileRequestDto
{
    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } =
        string.Empty;

    [MaxLength(30)]
    public string? Phone { get; set; }

    [MaxLength(150)]
    public string? CompanyName { get; set; }
}