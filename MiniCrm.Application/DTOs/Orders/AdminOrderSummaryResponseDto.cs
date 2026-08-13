namespace MiniCrm.Application.DTOs.Orders;

public class AdminOrderSummaryResponseDto
{
    public int Id { get; set; }

    public string OrderNumber { get; set; } =
        string.Empty;

    public int CustomerId { get; set; }

    public string CustomerName { get; set; } =
        string.Empty;

    public string CustomerEmail { get; set; } =
        string.Empty;

    public string Status { get; set; } =
        string.Empty;

    public decimal TotalAmount { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? ConfirmedAtUtc { get; set; }

    public DateTime? ShippedAtUtc { get; set; }

    public DateTime? DeliveredAtUtc { get; set; }

    public DateTime? CancelledAtUtc { get; set; }
}