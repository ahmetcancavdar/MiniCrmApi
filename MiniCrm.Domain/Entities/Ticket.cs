using MiniCrm.Domain.Common;
using MiniCrm.Domain.Enums;
using MiniCrm.Domain.Exceptions;

namespace MiniCrm.Domain.Entities;

public class Ticket : BaseEntity
{
    private readonly List<TicketMessage> _messages = new();

    public int CustomerId { get; private set; }

    public Customer Customer { get; private set; } = null!;

    public int? OrderId { get; private set; }

    public Order? Order { get; private set; }

    public string Subject { get; private set; } =
        string.Empty;

    public TicketPriority Priority { get; private set; }

    public TicketStatus Status { get; private set; }

    public IReadOnlyCollection<TicketMessage> Messages =>
        _messages.AsReadOnly();

    public DateTime? ResolvedAtUtc { get; private set; }

    public DateTime? ClosedAtUtc { get; private set; }


    private Ticket()
    {
    }


    public Ticket(
        int customerId,
        string subject,
        TicketPriority priority,
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
                "Ticket subject cannot be empty.");
        }

        if (!Enum.IsDefined(
                typeof(TicketPriority),
                priority))
        {
            throw new DomainException(
                "Invalid ticket priority.");
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

        Priority =
            priority;

        OrderId =
            orderId;

        Status =
            TicketStatus.Open;
    }


    // ============================================================
    // CUSTOMER MESSAGE
    // ============================================================

    public void AddCustomerMessage(
        Guid senderUserId,
        string message)
    {
        EnsureCanReceiveMessage();

        _messages.Add(
            new TicketMessage(
                this,
                senderUserId,
                MessageSenderType.Customer,
                message));

        if (Status is
            TicketStatus.WaitingForCustomer
            or TicketStatus.Resolved)
        {
            Status =
                TicketStatus.InProgress;

            ResolvedAtUtc =
                null;
        }
    }


    // ============================================================
    // ADMIN MESSAGE
    // ============================================================

    public void AddAdminMessage(
        Guid senderUserId,
        string message)
    {
        EnsureCanReceiveMessage();

        if (Status is
            TicketStatus.Open
            or TicketStatus.Resolved)
        {
            Status =
                TicketStatus.InProgress;

            ResolvedAtUtc =
                null;
        }

        _messages.Add(
            new TicketMessage(
                this,
                senderUserId,
                MessageSenderType.Admin,
                message));

        Status =
            TicketStatus.WaitingForCustomer;
    }


    // ============================================================
    // IN PROGRESS
    // ============================================================

    public void StartProgress()
    {
        if (Status ==
            TicketStatus.Closed)
        {
            throw new DomainException(
                "A closed ticket cannot be reopened.");
        }

        if (Status ==
            TicketStatus.InProgress)
        {
            return;
        }

        if (Status ==
            TicketStatus.Resolved)
        {
            ResolvedAtUtc =
                null;
        }

        Status =
            TicketStatus.InProgress;
    }


    // ============================================================
    // WAITING FOR CUSTOMER
    // ============================================================

    public void WaitForCustomer()
    {
        if (Status ==
            TicketStatus.WaitingForCustomer)
        {
            return;
        }

        if (Status !=
            TicketStatus.InProgress)
        {
            throw new DomainException(
                "Only an in-progress ticket can wait for the customer.");
        }

        Status =
            TicketStatus.WaitingForCustomer;
    }


    // ============================================================
    // RESOLVE
    // ============================================================

    public void Resolve(
        DateTime utcNow)
    {
        if (Status is not
            (TicketStatus.InProgress
            or TicketStatus.WaitingForCustomer))
        {
            throw new DomainException(
                "Only an active ticket can be resolved.");
        }

        Status =
            TicketStatus.Resolved;

        ResolvedAtUtc =
            utcNow;
    }


    // ============================================================
    // CLOSE
    // ============================================================

    public void Close(
        DateTime utcNow)
    {
        if (Status !=
            TicketStatus.Resolved)
        {
            throw new DomainException(
                "Only a resolved ticket can be closed.");
        }

        Status =
            TicketStatus.Closed;

        ClosedAtUtc =
            utcNow;
    }


    // ============================================================
    // VALIDATION
    // ============================================================

    private void EnsureCanReceiveMessage()
    {
        if (Status ==
            TicketStatus.Closed)
        {
            throw new DomainException(
                "A closed ticket cannot receive new messages.");
        }
    }
}