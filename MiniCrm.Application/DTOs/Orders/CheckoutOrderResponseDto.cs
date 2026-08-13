namespace MiniCrm.Application.DTOs.Orders;

public class CheckoutOrderResponseDto
{
    public OrderResponseDto Order { get; set; } =
        new();

    public DateTime VerificationExpiresAtUtc { get; set; }

    public bool VerificationEmailSent { get; set; }

    public string Message { get; set; } =
        string.Empty;
}