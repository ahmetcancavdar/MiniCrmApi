namespace MiniCrm.Application.DTOs.Orders;

public class VerificationEmailResponseDto
{
    public int OrderId { get; set; }

    public string OrderNumber { get; set; } =
        string.Empty;

    public DateTime ExpiresAtUtc { get; set; }

    public bool EmailSent { get; set; }

    public string Message { get; set; } =
        string.Empty;
}