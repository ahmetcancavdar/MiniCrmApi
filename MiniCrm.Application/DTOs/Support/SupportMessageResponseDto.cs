namespace MiniCrm.Application.DTOs.Support;

public class SupportMessageResponseDto
{
    public int Id { get; set; }

    public Guid SenderUserId { get; set; }

    public string SenderType { get; set; } =
        string.Empty;

    public string Message { get; set; } =
        string.Empty;

    public DateTime CreatedAtUtc { get; set; }
}