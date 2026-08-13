namespace MiniCrm.Application.DTOs.Orders;

public class AdminOrderDetailResponseDto
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } =
        string.Empty;

    public string CustomerEmail { get; set; } =
        string.Empty;

    public OrderResponseDto Order { get; set; } =
        new();
}