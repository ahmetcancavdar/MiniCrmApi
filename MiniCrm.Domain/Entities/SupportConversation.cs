using MiniCrm.Domain.Common;
using MiniCrm.Domain.Enums;
using MiniCrm.Domain.Exceptions;

namespace MiniCrm.Domain.Entities;

public class SupportConversation : BaseEntity
{
    private readonly List<SupportMessage> _messages = new();


    // ============================================================
    // CUSTOMER
    // ============================================================

    public int CustomerId { get; private set; }

    public Customer Customer { get; private set; } =
        null!;


    // ============================================================
    // ORDER (OPTIONAL)
    // ============================================================

    public int? OrderId { get; private set; }

    public Order? Order { get; private set; }


    // ============================================================
    // STATUS
    // ============================================================

    public SupportConversationStatus Status { get; private set; }


    // ============================================================
    // DATES
    // ============================================================

    public DateTime StartedAtUtc { get; private set; }

    public DateTime? ClosedAtUtc { get; private set; }


    // ============================================================
    // MESSAGES
    // ============================================================

    public IReadOnlyCollection<SupportMessage> Messages =>
        _messages.AsReadOnly();


    // ============================================================
    // EF CORE
    // ============================================================

    private SupportConversation()
    {
    }


    // ============================================================
    // CREATE
    // ============================================================

    public SupportConversation(
        int customerId,
        int? orderId = null)
    {
        if (customerId <= 0)
        {
            throw new DomainException(
                "A valid customer is required.");
        }

        if (orderId.HasValue &&
            orderId.Value <= 0)
        {
            throw new DomainException(
                "Invalid order.");
        }

        CustomerId =
            customerId;

        OrderId =
            orderId;

        Status =
            SupportConversationStatus.Open;

        StartedAtUtc =
            DateTime.UtcNow;

        ClosedAtUtc =
            null;
    }


    // ============================================================
    // CUSTOMER MESSAGE
    // ============================================================

    public void AddCustomerMessage(
        Guid senderUserId,
        string message)
    {
        EnsureOpen();

        _messages.Add(
            new SupportMessage(
                this,
                senderUserId,
                MessageSenderType.Customer,
                message));
    }


    // ============================================================
    // ADMIN MESSAGE
    // ============================================================

    public void AddAdminMessage(
        Guid senderUserId,
        string message)
    {
        EnsureOpen();

        _messages.Add(
            new SupportMessage(
                this,
                senderUserId,
                MessageSenderType.Admin,
                message));
    }


    // ============================================================
    // CLOSE
    // ============================================================

    public void Close()
    {
        if (Status ==
            SupportConversationStatus.Closed)
        {
            throw new DomainException(
                "Support conversation is already closed.");
        }

        Status =
            SupportConversationStatus.Closed;

        ClosedAtUtc =
            DateTime.UtcNow;
    }


    // ============================================================
    // VALIDATION
    // ============================================================

    private void EnsureOpen()
    {
        if (Status !=
            SupportConversationStatus.Open)
        {
            throw new DomainException(
                "A closed support conversation cannot receive messages.");
        }
    }
}