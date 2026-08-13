using MiniCrm.Domain.Common;
using MiniCrm.Domain.Enums;
using MiniCrm.Domain.Exceptions;

namespace MiniCrm.Domain.Entities;

public class AfterSalesRequest : BaseEntity
{
    // ============================================================
    // CUSTOMER
    // ============================================================

    public int CustomerId { get; private set; }

    public Customer Customer { get; private set; } =
        null!;


    // ============================================================
    // ORDER ITEM
    // ============================================================

    public int OrderItemId { get; private set; }

    public OrderItem OrderItem { get; private set; } =
        null!;


    // ============================================================
    // REQUEST
    // ============================================================

    public AfterSalesRequestType RequestType { get; private set; }

    public AfterSalesRequestStatus Status { get; private set; }

    public int Quantity { get; private set; }

    public string Reason { get; private set; } =
        string.Empty;


    // ============================================================
    // ADMIN
    // ============================================================

    public string? AdminNote { get; private set; }


    // ============================================================
    // DATES
    // ============================================================

    public DateTime? ReviewedAtUtc { get; private set; }

    public DateTime? CompletedAtUtc { get; private set; }

    public DateTime? CancelledAtUtc { get; private set; }


    // ============================================================
    // EF CORE
    // ============================================================

    private AfterSalesRequest()
    {
    }


    // ============================================================
    // CREATE
    // ============================================================

    public AfterSalesRequest(
        int customerId,
        int orderItemId,
        AfterSalesRequestType requestType,
        int quantity,
        string reason)
    {
        if (customerId <= 0)
        {
            throw new DomainException(
                "A valid customer is required.");
        }

        if (orderItemId <= 0)
        {
            throw new DomainException(
                "A valid order item is required.");
        }

        if (!Enum.IsDefined(
                typeof(AfterSalesRequestType),
                requestType))
        {
            throw new DomainException(
                "Invalid after-sales request type.");
        }

        if (quantity <= 0)
        {
            throw new DomainException(
                "Quantity must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new DomainException(
                "Reason cannot be empty.");
        }

        var normalizedReason =
            reason.Trim();

        if (normalizedReason.Length > 2000)
        {
            throw new DomainException(
                "Reason cannot exceed 2000 characters.");
        }

        CustomerId =
            customerId;

        OrderItemId =
            orderItemId;

        RequestType =
            requestType;

        Quantity =
            quantity;

        Reason =
            normalizedReason;

        Status =
            AfterSalesRequestStatus.Requested;
    }


    // ============================================================
    // START REVIEW
    // ============================================================

    public void StartReview(
        DateTime utcNow)
    {
        if (Status !=
            AfterSalesRequestStatus.Requested)
        {
            throw new DomainException(
                "Only a requested after-sales request can be taken under review.");
        }

        Status =
            AfterSalesRequestStatus.UnderReview;

        ReviewedAtUtc =
            utcNow;
    }


    // ============================================================
    // APPROVE
    // ============================================================

    public void Approve(
        string adminNote)
    {
        if (Status !=
            AfterSalesRequestStatus.UnderReview)
        {
            throw new DomainException(
                "Only a request under review can be approved.");
        }

        AdminNote =
            NormalizeAdminNote(
                adminNote);

        Status =
            AfterSalesRequestStatus.Approved;
    }


    // ============================================================
    // REJECT
    // ============================================================

    public void Reject(
        string adminNote)
    {
        if (Status !=
            AfterSalesRequestStatus.UnderReview)
        {
            throw new DomainException(
                "Only a request under review can be rejected.");
        }

        AdminNote =
            NormalizeAdminNote(
                adminNote);

        Status =
            AfterSalesRequestStatus.Rejected;
    }


    // ============================================================
    // COMPLETE
    // ============================================================

    public void Complete(
        DateTime utcNow)
    {
        if (Status !=
            AfterSalesRequestStatus.Approved)
        {
            throw new DomainException(
                "Only an approved after-sales request can be completed.");
        }

        Status =
            AfterSalesRequestStatus.Completed;

        CompletedAtUtc =
            utcNow;
    }


    // ============================================================
    // CUSTOMER CANCEL
    // ============================================================

    public void Cancel(
        DateTime utcNow)
    {
        if (Status !=
            AfterSalesRequestStatus.Requested)
        {
            throw new DomainException(
                "Only a newly requested after-sales request can be cancelled by the customer.");
        }

        Status =
            AfterSalesRequestStatus.Cancelled;

        CancelledAtUtc =
            utcNow;
    }


    // ============================================================
    // ADMIN NOTE
    // ============================================================

    private static string NormalizeAdminNote(
        string adminNote)
    {
        if (string.IsNullOrWhiteSpace(
                adminNote))
        {
            throw new DomainException(
                "Admin note is required.");
        }

        var normalized =
            adminNote.Trim();

        if (normalized.Length > 2000)
        {
            throw new DomainException(
                "Admin note cannot exceed 2000 characters.");
        }

        return normalized;
    }
}