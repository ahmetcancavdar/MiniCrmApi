namespace MiniCrm.Application.DTOs.Support;

public class SupportConversationDetailResponseDto
{
    public int Id { get; set; }

    public string Status { get; set; } =
        string.Empty;

    public int? OrderId { get; set; }

    public string? OrderNumber { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public List<SupportMessageResponseDto> Messages { get; set; } =
        new();
}
