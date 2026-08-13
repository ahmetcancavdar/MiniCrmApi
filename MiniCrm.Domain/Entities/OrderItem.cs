using MiniCrm.Domain.Common;
using MiniCrm.Domain.Exceptions;

namespace MiniCrm.Domain.Entities;

public class OrderItem : BaseEntity
{
    public int OrderId { get; private set; }

    public Order Order { get; private set; } = null!;

    public int ProductId { get; private set; }

    public Product Product { get; private set; } = null!;

    public string ProductName { get; private set; } = string.Empty;

    public string SKU { get; private set; } = string.Empty;

    public int Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public decimal LineTotal => Quantity * UnitPrice;

    private OrderItem()
    {
    }

    internal OrderItem(
        Order order,
        int productId,
        string productName,
        string sku,
        decimal unitPrice,
        int quantity)
    {
        if (productId <= 0)
        {
            throw new DomainException(
                "A valid product is required.");
        }

        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new DomainException(
                "Product name cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new DomainException(
                "SKU cannot be empty.");
        }

        if (unitPrice < 0)
        {
            throw new DomainException(
                "Unit price cannot be negative.");
        }

        if (quantity <= 0)
        {
            throw new DomainException(
                "Quantity must be greater than zero.");
        }

        Order = order;
        OrderId = order.Id;

        ProductId = productId;
        ProductName = productName.Trim();
        SKU = sku.Trim();
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    internal void IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException(
                "Quantity increase must be greater than zero.");
        }

        Quantity += quantity;
    }
}