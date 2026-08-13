using System.ComponentModel.DataAnnotations;

namespace MiniCrm.Application.DTOs.Categories;

public class UpdateCategoryRequestDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}