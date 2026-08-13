namespace MiniCrm.Application.DTOs.Orders;

public class OrderSummaryResponseDto
{
    public int Id { get; set; }

    public string OrderNumber { get; set; } =
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