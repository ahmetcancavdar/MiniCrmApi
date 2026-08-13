namespace MiniCrm.Application.DTOs.Support;

public class AdminSupportConversationDetailResponseDto
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } =
        string.Empty;

    public string CustomerEmail { get; set; } =
        string.Empty;

    public SupportConversationDetailResponseDto Conversation { get; set; } =
        new();
}