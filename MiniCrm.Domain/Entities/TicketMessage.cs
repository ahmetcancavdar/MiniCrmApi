using MiniCrm.Domain.Common;
using MiniCrm.Domain.Enums;
using MiniCrm.Domain.Exceptions;

namespace MiniCrm.Domain.Entities;

public class TicketMessage : BaseEntity
{
    public int TicketId { get; private set; }

    public Ticket Ticket { get; private set; } =
        null!;

    public Guid SenderUserId { get; private set; }

    public MessageSenderType SenderType { get; private set; }

    public string Message { get; private set; } =
        string.Empty;


    private TicketMessage()
    {
    }


    internal TicketMessage(
        Ticket ticket,
        Guid senderUserId,
        MessageSenderType senderType,
        string message)
    {
        if (ticket is null)
        {
            throw new DomainException(
                "Ticket is required.");
        }

        if (senderUserId ==
            Guid.Empty)
        {
            throw new DomainException(
                "A valid sender is required.");
        }

        if (!Enum.IsDefined(
                typeof(MessageSenderType),
                senderType))
        {
            throw new DomainException(
                "Invalid message sender type.");
        }

        if (string.IsNullOrWhiteSpace(
                message))
        {
            throw new DomainException(
                "Ticket message cannot be empty.");
        }

        Ticket =
            ticket;

        TicketId =
            ticket.Id;

        SenderUserId =
            senderUserId;

        SenderType =
            senderType;

        Message =
            message.Trim();
    }
}