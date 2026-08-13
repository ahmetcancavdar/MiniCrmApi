using System.ComponentModel.DataAnnotations;

namespace MiniCrm.Application.DTOs.Products;

public class UpdateProductRequestDto
{
    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(64)]
    public string SKU { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [MaxLength(2048)]
    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;
}