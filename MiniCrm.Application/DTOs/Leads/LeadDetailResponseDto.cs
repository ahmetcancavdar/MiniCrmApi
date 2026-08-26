namespace MiniCrm.Application.DTOs.Leads;

public class LeadDetailResponseDto : LeadResponseDto
{
    public List<LeadNoteResponseDto> LeadNotes { get; set; } = new();
}
