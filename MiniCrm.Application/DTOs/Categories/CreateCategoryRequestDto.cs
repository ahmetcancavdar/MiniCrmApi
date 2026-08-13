using System.ComponentModel.DataAnnotations;

namespace MiniCrm.Application.DTOs.Categories;

public class CreateCategoryRequestDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }
}