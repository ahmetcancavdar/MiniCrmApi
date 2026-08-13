using System.ComponentModel.DataAnnotations;

namespace MiniCrm.Application.DTOs.Cart;

public class UpdateCartItemQuantityRequestDto
{
    [Range(1, 1000)]
    public int Quantity { get; set; }
}