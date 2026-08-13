namespace MiniCrm.Application.DTOs.Orders;

public class OrderResponseDto
{
    public int Id { get; set; }

    public string OrderNumber { get; set; } =
        string.Empty;

    public string Status { get; set; } =
        string.Empty;

    public decimal TotalAmount { get; set; }

    public DateTime CreatedAtUtc { get; set; }


    // ============================================================
    // SHIPPING ADDRESS
    // ============================================================

    public string RecipientName { get; set; } =
        string.Empty;

    public string? Phone { get; set; }

    public string AddressLine { get; set; } =
        string.Empty;

    public string City { get; set; } =
        string.Empty;

    public string District { get; set; } =
        string.Empty;

    public string? PostalCode { get; set; }

    public string Country { get; set; } =
        string.Empty;


    // ============================================================
    // VERIFICATION / STATUS DATES
    // ============================================================

    public DateTime? VerificationExpiresAtUtc { get; set; }

    public DateTime? ConfirmedAtUtc { get; set; }

    public DateTime? ShippedAtUtc { get; set; }

    public DateTime? DeliveredAtUtc { get; set; }

    public DateTime? CancelledAtUtc { get; set; }

    public string? CancellationReason { get; set; }


    // ============================================================
    // ITEMS
    // ============================================================

    public List<OrderItemResponseDto> Items { get; set; } =
        new();
}