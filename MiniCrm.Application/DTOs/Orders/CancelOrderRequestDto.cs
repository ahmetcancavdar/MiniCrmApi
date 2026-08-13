using System.ComponentModel.DataAnnotations;

namespace MiniCrm.Application.DTOs.Orders;

public class CancelOrderRequestDto
{
    [Required]
    [MaxLength(500)]
    public string Reason { get; set; } =
        string.Empty;
}