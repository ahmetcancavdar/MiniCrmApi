using MiniCrm.Domain.Common;
using MiniCrm.Domain.Exceptions;

namespace MiniCrm.Domain.Entities;

public class LeadNote : BaseEntity
{
    public int LeadId { get; private set; }

    public Lead Lead { get; private set; } = null!;

    public Guid AdminUserId { get; private set; }

    public string Note { get; private set; } = string.Empty;

    private LeadNote()
    {
    }

    internal LeadNote(
        Lead lead,
        Guid adminUserId,
        string note)
    {
        if (lead is null)
        {
            throw new DomainException("A valid lead is required.");
        }

        if (adminUserId == Guid.Empty)
        {
            throw new DomainException("A valid admin user is required.");
        }

        if (string.IsNullOrWhiteSpace(note))
        {
            throw new DomainException("Note cannot be empty.");
        }

        Lead = lead;
        LeadId = lead.Id;
        AdminUserId = adminUserId;
        Note = note.Trim();
    }
}
