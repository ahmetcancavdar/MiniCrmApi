namespace MiniCrm.Application.DTOs.Support;

public class SupportConversationSummaryResponseDto
{
    public int Id { get; set; }

    public string Status { get; set; } =
        string.Empty;

    public int? OrderId { get; set; }

    public string? OrderNumber { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}
