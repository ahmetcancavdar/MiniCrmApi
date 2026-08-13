using MiniCrm.Domain.Common;
using MiniCrm.Domain.Enums;
using MiniCrm.Domain.Exceptions;

namespace MiniCrm.Domain.Entities;

public class Complaint : BaseEntity
{
    public int CustomerId { get; private set; }

    public Customer Customer { get; private set; } =
        null!;

    public int? OrderId { get; private set; }

    public Order? Order { get; private set; }


    // ============================================================
    // COMPLAINT CONTENT
    // ============================================================

    public string Subject { get; private set; } =
        string.Empty;

    public string Description { get; private set; } =
        string.Empty;


    // ============================================================
    // STATUS
    // ============================================================

    public ComplaintStatus Status { get; private set; }


    // ============================================================
    // ADMIN
    // ============================================================

    public string? AdminNote { get; private set; }


    // ============================================================
    // STATUS DATES
    // ============================================================

    public DateTime? ReviewedAtUtc { get; private set; }

    public DateTime? ResolvedAtUtc { get; private set; }

    public DateTime? RejectedAtUtc { get; private set; }

    public DateTime? ClosedAtUtc { get; private set; }


    private Complaint()
    {
    }


    public Complaint(
        int customerId,
        string subject,
        string description,
        int? orderId = null)
    {
        if (customerId <= 0)
        {
            throw new DomainException(
                "A valid customer is required.");
        }

        if (string.IsNullOrWhiteSpace(subject))
        {
            throw new DomainException(
                "Complaint subject cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException(
                "Complaint description cannot be empty.");
        }

        if (orderId.HasValue &&
            orderId.Value <= 0)
        {
            throw new DomainException(
                "Invalid order.");
        }

        CustomerId =
            customerId;

        Subject =
            subject.Trim();

        Description =
            description.Trim();

        OrderId =
            orderId;

        Status =
            ComplaintStatus.Open;
    }


    // ============================================================
    // START REVIEW
    // ============================================================

    public void StartReview(
        DateTime utcNow)
    {
        if (Status !=
            ComplaintStatus.Open)
        {
            throw new DomainException(
                "Only an open complaint can be taken under review.");
        }

        Status =
            ComplaintStatus.UnderReview;

        ReviewedAtUtc =
            utcNow;
    }


    // ============================================================
    // RESOLVE
    // ============================================================

    public void Resolve(
        string adminNote,
        DateTime utcNow)
    {
        if (Status !=
            ComplaintStatus.UnderReview)
        {
            throw new DomainException(
                "Only a complaint under review can be resolved.");
        }

        if (string.IsNullOrWhiteSpace(
                adminNote))
        {
            throw new DomainException(
                "Admin note is required when resolving a complaint.");
        }

        Status =
            ComplaintStatus.Resolved;

        AdminNote =
            adminNote.Trim();

        ResolvedAtUtc =
            utcNow;

        RejectedAtUtc =
            null;
    }


    // ============================================================
    // REJECT
    // ============================================================

    public void Reject(
        string adminNote,
        DateTime utcNow)
    {
        if (Status !=
            ComplaintStatus.UnderReview)
        {
            throw new DomainException(
                "Only a complaint under review can be rejected.");
        }

        if (string.IsNullOrWhiteSpace(
                adminNote))
        {
            throw new DomainException(
                "Admin note is required when rejecting a complaint.");
        }

        Status =
            ComplaintStatus.Rejected;

        AdminNote =
            adminNote.Trim();

        RejectedAtUtc =
            utcNow;

        ResolvedAtUtc =
            null;
    }


    // ============================================================
    // CLOSE
    // ============================================================

    public void Close(
        DateTime utcNow)
    {
        if (Status is not
            (ComplaintStatus.Resolved
            or ComplaintStatus.Rejected))
        {
            throw new DomainException(
                "Only a resolved or rejected complaint can be closed.");
        }

        Status =
            ComplaintStatus.Closed;

        ClosedAtUtc =
            utcNow;
    }
}