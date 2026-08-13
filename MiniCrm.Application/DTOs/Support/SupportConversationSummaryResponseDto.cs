namespace MiniCrm.Application.DTOs.Support;

public class SupportConversationSummaryResponseDto
{
    public int Id { get; set; }

    public string Status { get; set; } =
        string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}