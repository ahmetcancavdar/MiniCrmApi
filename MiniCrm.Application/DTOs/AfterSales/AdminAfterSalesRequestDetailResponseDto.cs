namespace MiniCrm.Application.DTOs.AfterSales;

public class AdminAfterSalesRequestDetailResponseDto
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } =
        string.Empty;

    public string CustomerEmail { get; set; } =
        string.Empty;

    public AfterSalesRequestDetailResponseDto Request { get; set; } =
        new();
}