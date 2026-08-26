using MiniCrm.Domain.Common;
using MiniCrm.Domain.Enums;
using MiniCrm.Domain.Exceptions;

namespace MiniCrm.Domain.Entities;

public class Lead : BaseEntity
{
    public string FullName { get; private set; } = string.Empty;

    public string? CompanyName { get; private set; }

    public string Email { get; private set; } = string.Empty;

    public string? Phone { get; private set; }

    public LeadSource Source { get; private set; }

    public LeadStatus Status { get; private set; }

    public string? InterestArea { get; private set; }

    public string? Notes { get; private set; }

    public Guid? AssignedAdminUserId { get; private set; }

    public DateTime? NextFollowUpDate { get; private set; }

    public int? ConvertedCustomerId { get; private set; }

    public Customer? ConvertedCustomer { get; private set; }

    private readonly List<LeadNote> _leadNotes = new();

    public IReadOnlyCollection<LeadNote> LeadNotes => _leadNotes.AsReadOnly();

    private Lead()
    {
    }

    public Lead(
        string fullName,
        string? companyName,
        string email,
        string? phone,
        LeadSource source,
        string? interestArea,
        string? notes,
        DateTime? nextFollowUpDate)
    {
        ChangeFullName(fullName);
        ChangeCompanyName(companyName);
        ChangeEmail(email);
        ChangePhone(phone);
        ChangeSource(source);
        ChangeInterestArea(interestArea);
        ChangeNotes(notes);

        NextFollowUpDate = nextFollowUpDate;

        Status = LeadStatus.New;
    }

    public void Update(
        string fullName,
        string? companyName,
        string email,
        string? phone,
        LeadSource source,
        string? interestArea,
        string? notes,
        DateTime? nextFollowUpDate)
    {
        EnsureNotConverted();

        ChangeFullName(fullName);
        ChangeCompanyName(companyName);
        ChangeEmail(email);
        ChangePhone(phone);
        ChangeSource(source);
        ChangeInterestArea(interestArea);
        ChangeNotes(notes);

        NextFollowUpDate = nextFollowUpDate;
    }

    public void MarkAsContacted()
    {
        EnsureNotConverted();
        EnsureNotLost();

        Status = LeadStatus.Contacted;
    }

    public void MarkAsQualified()
    {
        EnsureNotConverted();
        EnsureNotLost();

        Status = LeadStatus.Qualified;
    }

    public void MarkProposalSent()
    {
        EnsureNotConverted();
        EnsureNotLost();

        Status = LeadStatus.ProposalSent;
    }

    public void MarkAsLost(string? reason)
    {
        EnsureNotConverted();

        if (!string.IsNullOrWhiteSpace(reason))
        {
            var lostNote = $"[Lost] {reason.Trim()}";

            Notes = string.IsNullOrWhiteSpace(Notes)
                ? lostNote
                : $"{Notes}\n{lostNote}";
        }

        Status = LeadStatus.Lost;
    }

    public void ConvertToCustomer(int customerId)
    {
        EnsureNotConverted();

        if (customerId <= 0)
        {
            throw new DomainException("A valid customer is required.");
        }

        ConvertedCustomerId = customerId;
        Status = LeadStatus.Converted;
    }

    public void AssignToAdmin(Guid adminUserId)
    {
        if (adminUserId == Guid.Empty)
        {
            throw new DomainException("A valid admin user is required.");
        }

        AssignedAdminUserId = adminUserId;
    }

    public void SetNextFollowUpDate(DateTime? nextFollowUpDate)
    {
        NextFollowUpDate = nextFollowUpDate;
    }

    public void AddNote(Guid adminUserId, string note)
    {
        var leadNote = new LeadNote(this, adminUserId, note);

        _leadNotes.Add(leadNote);
    }

    public void SoftDelete()
    {
        IsDeleted = true;
    }

    private void ChangeFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new DomainException("Lead full name cannot be empty.");
        }

        FullName = fullName.Trim();
    }

    private void ChangeCompanyName(string? companyName)
    {
        CompanyName = string.IsNullOrWhiteSpace(companyName)
            ? null
            : companyName.Trim();
    }

    private void ChangeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException("Lead email cannot be empty.");
        }

        Email = email.Trim().ToLowerInvariant();
    }

    private void ChangePhone(string? phone)
    {
        Phone = string.IsNullOrWhiteSpace(phone)
            ? null
            : phone.Trim();
    }

    private void ChangeSource(LeadSource source)
    {
        if (!Enum.IsDefined(source))
        {
            throw new DomainException("A valid lead source is required.");
        }

        Source = source;
    }

    private void ChangeInterestArea(string? interestArea)
    {
        InterestArea = string.IsNullOrWhiteSpace(interestArea)
            ? null
            : interestArea.Trim();
    }

    private void ChangeNotes(string? notes)
    {
        Notes = string.IsNullOrWhiteSpace(notes)
            ? null
            : notes.Trim();
    }

    private void EnsureNotConverted()
    {
        if (Status == LeadStatus.Converted)
        {
            throw new DomainException("A converted lead cannot be modified.");
        }
    }

    private void EnsureNotLost()
    {
        if (Status == LeadStatus.Lost)
        {
            throw new DomainException("A lost lead cannot change status this way.");
        }
    }
}
