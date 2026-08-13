using MiniCrm.Domain.Common;
using MiniCrm.Domain.Enums;
using MiniCrm.Domain.Exceptions;

namespace MiniCrm.Domain.Entities;

public class SupportMessage : BaseEntity
{
    public int SupportConversationId { get; private set; }

    public SupportConversation SupportConversation { get; private set; } =
        null!;

    public Guid SenderUserId { get; private set; }

    public MessageSenderType SenderType { get; private set; }

    public string Message { get; private set; } =
        string.Empty;


    private SupportMessage()
    {
    }


    internal SupportMessage(
        SupportConversation supportConversation,
        Guid senderUserId,
        MessageSenderType senderType,
        string message)
    {
        if (supportConversation is null)
        {
            throw new DomainException(
                "Support conversation is required.");
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
                "Support message cannot be empty.");
        }

        SupportConversation =
            supportConversation;

        SupportConversationId =
            supportConversation.Id;

        SenderUserId =
            senderUserId;

        SenderType =
            senderType;

        Message =
            message.Trim();
    }
}