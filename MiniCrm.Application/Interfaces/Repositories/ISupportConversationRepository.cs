using MiniCrm.Domain.Entities;

namespace MiniCrm.Application.Interfaces.Repositories;

public interface ISupportConversationRepository
{
    Task<SupportConversation?> GetWithDetailsAsync(
        int conversationId,
        CancellationToken cancellationToken = default);

    Task<List<SupportConversation>> GetByCustomerIdAsync(
        int customerId,
        CancellationToken cancellationToken = default);

    Task<List<SupportConversation>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        SupportConversation conversation,
        CancellationToken cancellationToken = default);
}