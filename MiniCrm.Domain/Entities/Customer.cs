using MiniCrm.Domain.Common;

namespace MiniCrm.Domain.Entities;

public class Customer : BaseEntity
{
    public Guid UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string? CompanyName { get; set; }

    public Cart? Cart { get; private set; }

    public List<CustomerAddress> Addresses { get; set; } = new();

    public List<Order> Orders { get; set; } = new();

    public List<SupportConversation> SupportConversations { get; set; } = new();

    public List<EmailLog> EmailLogs { get; set; } = new();
}