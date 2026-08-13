namespace MiniCrm.Application.DTOs.Cart;

public class CartResponseDto
{
    public int CartId { get; set; }

    public int CustomerId { get; set; }

    public int TotalItemCount { get; set; }

    public decimal TotalAmount { get; set; }

    public List<CartItemResponseDto> Items { get; set; } =
        new();
}