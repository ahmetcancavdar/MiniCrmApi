namespace MiniCrm.Application.DTOs.Tickets;

public class AdminTicketSummaryResponseDto
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string CustomerName { get; set; } =
        string.Empty;

    public string CustomerEmail { get; set; } =
        string.Empty;

    public string Subject { get; set; } =
        string.Empty;

    public string Priority { get; set; } =
        string.Empty;

    public string Status { get; set; } =
        string.Empty;

    public int? OrderId { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}