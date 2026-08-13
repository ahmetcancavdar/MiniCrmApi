using System.ComponentModel.DataAnnotations;

namespace MiniCrm.Application.DTOs.Products;

public class AdjustStockRequestDto
{
    [Range(-1000000, 1000000)]
    public int QuantityChange { get; set; }

    [MaxLength(500)]
    public string? Note { get; set; }
}