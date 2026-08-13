namespace MiniCrm.Application.DTOs.Cart;

public class CartItemResponseDto
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } =
        string.Empty;

    public string SKU { get; set; } =
        string.Empty;

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal LineTotal { get; set; }

    public int AvailableStock { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsAvailable { get; set; }
}