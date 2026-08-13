using System.ComponentModel.DataAnnotations;
using MiniCrm.Domain.Enums;

namespace MiniCrm.Application.DTOs.AfterSales;

public class CreateAfterSalesRequestDto
{
    [Range(1, int.MaxValue)]
    public int OrderId { get; set; }

    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    public AfterSalesRequestType RequestType { get; set; }

    [Range(1, 1000)]
    public int Quantity { get; set; }

    [Required]
    [MaxLength(2000)]
    public string Reason { get; set; } =
        string.Empty;
}