namespace MiniCrm.Application.DTOs.Complaints;

public class ComplaintDetailResponseDto
{
    public int Id { get; set; }

    public string Subject { get; set; } =
        string.Empty;

    public string Description { get; set; } =
        string.Empty;

    public string Status { get; set; } =
        string.Empty;

    public int? OrderId { get; set; }

    public string? OrderNumber { get; set; }

    public string? AdminNote { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public DateTime? ReviewedAtUtc { get; set; }

    public DateTime? ResolvedAtUtc { get; set; }

    public DateTime? RejectedAtUtc { get; set; }

    public DateTime? ClosedAtUtc { get; set; }
}