using MiniCrm.Domain.Common;

namespace MiniCrm.Domain.Entities;

public class CustomerAddress : BaseEntity
{
    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;

    public string Title { get; set; } = string.Empty;

    public string AddressLine { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string District { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public bool IsDefault { get; set; }
}