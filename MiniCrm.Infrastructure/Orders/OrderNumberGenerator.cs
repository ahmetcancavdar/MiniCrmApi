using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Infrastructure.Orders;

public sealed class OrderNumberGenerator
    : IOrderNumberGenerator
{
    public string Generate()
    {
        var date =
            DateTime.UtcNow.ToString(
                "yyyyMMdd");

        var randomPart =
            Guid.NewGuid()
                .ToString("N")[..8]
                .ToUpperInvariant();

        return $"ORD-{date}-{randomPart}";
    }
}