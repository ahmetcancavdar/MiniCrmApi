using MiniCrm.Domain.Common;
using MiniCrm.Domain.Enums;
using MiniCrm.Domain.Exceptions;

namespace MiniCrm.Domain.Entities;

public class EmailLog : BaseEntity
{
    public int? CustomerId { get; private set; }

    public Customer? Customer { get; private set; }

    public string ToEmail { get; private set; } =
        string.Empty;

    public string Subject { get; private set; } =
        string.Empty;

    public string Body { get; private set; } =
        string.Empty;

    public EmailType EmailType { get; private set; }

    public EmailDeliveryStatus DeliveryStatus { get; private set; }

    public int? OrderId { get; private set; }

    public int? TicketId { get; private set; }

    public int? ComplaintId { get; private set; }

    public int? AfterSalesRequestId { get; private set; }

    public DateTime? SentAtUtc { get; private set; }

    public DateTime? FailedAtUtc { get; private set; }

    public string? FailureReason { get; private set; }


    private EmailLog()
    {
    }


    public EmailLog(
        string toEmail,
        string subject,
        string body,
        EmailType emailType,
        int? customerId = null,
        int? orderId = null,
        int? ticketId = null,
        int? complaintId = null,
        int? afterSalesRequestId = null)
    {
        if (string.IsNullOrWhiteSpace(
                toEmail))
        {
            throw new DomainException(
                "Email address cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(
                subject))
        {
            throw new DomainException(
                "Email subject cannot be empty.");
        }

        ToEmail =
            toEmail.Trim();

        Subject =
            subject.Trim();

        Body =
            body;

        EmailType =
            emailType;

        CustomerId =
            customerId;

        OrderId =
            orderId;

        TicketId =
            ticketId;

        ComplaintId =
            complaintId;

        AfterSalesRequestId =
            afterSalesRequestId;

        DeliveryStatus =
            EmailDeliveryStatus.Pending;
    }


    public void MarkAsSent(
        DateTime utcNow)
    {
        DeliveryStatus =
            EmailDeliveryStatus.Sent;

        SentAtUtc =
            utcNow;

        FailedAtUtc =
            null;

        FailureReason =
            null;
    }


    public void MarkAsFailed(
        string failureReason,
        DateTime utcNow)
    {
        DeliveryStatus =
            EmailDeliveryStatus.Failed;

        FailedAtUtc =
            utcNow;

        FailureReason =
            string.IsNullOrWhiteSpace(
                failureReason)
                ? "Unknown email error."
                : failureReason.Length > 2000
                    ? failureReason[..2000]
                    : failureReason;
    }
}