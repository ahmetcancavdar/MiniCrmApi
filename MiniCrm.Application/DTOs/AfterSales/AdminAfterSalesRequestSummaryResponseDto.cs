namespace MiniCrm.Application.DTOs.AfterSales;

public class AdminAfterSalesRequestSummaryResponseDto
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string CustomerName { get; set; } =
        string.Empty;

    public string CustomerEmail { get; set; } =
        string.Empty;

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

    public int RequestedQuantity { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}