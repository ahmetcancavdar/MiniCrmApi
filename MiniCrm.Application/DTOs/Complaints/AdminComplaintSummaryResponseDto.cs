namespace MiniCrm.Application.DTOs.Complaints;

public class AdminComplaintSummaryResponseDto
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string CustomerName { get; set; } =
        string.Empty;

    public string CustomerEmail { get; set; } =
        string.Empty;

    public string Subject { get; set; } =
        string.Empty;

    public string Status { get; set; } =
        string.Empty;

    public int? OrderId { get; set; }

    public string? OrderNumber { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}