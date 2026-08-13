namespace MiniCrm.Application.DTOs.Complaints;

public class AdminComplaintDetailResponseDto
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } =
        string.Empty;

    public string CustomerEmail { get; set; } =
        string.Empty;

    public ComplaintDetailResponseDto Complaint { get; set; } =
        new();
}