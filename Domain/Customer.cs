namespace MiniCrmApi.Domain;

public class Customer : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? CompanyName { get; set; }

    public List<Order> Orders { get; set; } = new();
}