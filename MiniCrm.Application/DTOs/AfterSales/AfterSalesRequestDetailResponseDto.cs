namespace MiniCrm.Application.DTOs.AfterSales;

public class AfterSalesRequestDetailResponseDto
{
	public int Id { get; set; }

	public int OrderItemId { get; set; }

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

	public int PurchasedQuantity { get; set; }

	public int RequestedQuantity { get; set; }

	public string Reason { get; set; } =
		string.Empty;

	public string? AdminNote { get; set; }

	public DateTime CreatedAtUtc { get; set; }

	public DateTime? UpdatedAtUtc { get; set; }

	public DateTime? ReviewedAtUtc { get; set; }

	public DateTime? CompletedAtUtc { get; set; }

	public DateTime? CancelledAtUtc { get; set; }
}