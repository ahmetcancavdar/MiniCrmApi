namespace MiniCrm.Application.DTOs.Leads;

public class LeadNoteResponseDto
{
    public int Id { get; set; }

    public Guid AdminUserId { get; set; }

    public string Note { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }
}
