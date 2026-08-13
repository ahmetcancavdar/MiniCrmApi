namespace MiniCrm.Application.DTOs.AfterSales;

public class AfterSalesRequestSummaryResponseDto
{
    public int Id { get; set; }

    public string RequestType { get; set; } =
        string.Empty;

    public string Status { get; set; } =
        string.Empty;

    public int OrderId { get; set; }

    public string OrderNumber { get; set; } =
        string.Empty;

    public int ProductId { get; set; }

    public string ProductName { get; set; } =
        string.Empty;

    public string SKU { get; set; } =
        string.Empty;

    public int RequestedQuantity { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? ReviewedAtUtc { get; set; }

    public DateTime? CompletedAtUtc { get; set; }

    public DateTime? CancelledAtUtc { get; set; }
}