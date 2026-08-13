namespace MiniCrm.Application.DTOs.Tickets;

public class TicketSummaryResponseDto
{
    public int Id { get; set; }

    public string Subject { get; set; } =
        string.Empty;

    public string Priority { get; set; } =
        string.Empty;

    public string Status { get; set; } =
        string.Empty;

    public int? OrderId { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public DateTime? ResolvedAtUtc { get; set; }

    public DateTime? ClosedAtUtc { get; set; }
}