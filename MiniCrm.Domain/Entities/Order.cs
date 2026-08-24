using MiniCrm.Domain.Common;
using MiniCrm.Domain.Enums;
using MiniCrm.Domain.Exceptions;
using MiniCrm.Domain.ValueObjects;

namespace MiniCrm.Domain.Entities;

public class Order : BaseEntity
{
    private readonly List<OrderItem> _items = new();

    public int CustomerId { get; private set; }

    public Customer Customer { get; private set; } = null!;

    public string OrderNumber { get; private set; } = string.Empty;

    public OrderStatus Status { get; private set; }

    public decimal TotalAmount { get; private set; }

    public OrderAddress ShippingAddress { get; private set; } = null!;

    public IReadOnlyCollection<OrderItem> Items =>
        _items.AsReadOnly();

    public OrderVerification? Verification { get; private set; }

    public DateTime? ConfirmedAtUtc { get; private set; }

    public DateTime? ShippedAtUtc { get; private set; }

    public DateTime? DeliveredAtUtc { get; private set; }

    public DateTime? CancelledAtUtc { get; private set; }

    public string? CancellationReason { get; private set; }

    private Order()
    {
    }

    public Order(
        int customerId,
        string orderNumber,
        OrderAddress shippingAddress)
    {
        if (customerId <= 0)
        {
            throw new DomainException(
                "A valid customer is required.");
        }

        if (string.IsNullOrWhiteSpace(orderNumber))
        {
            throw new DomainException(
                "Order number cannot be empty.");
        }

        CustomerId = customerId;

        OrderNumber =
            orderNumber.Trim();

        ShippingAddress =
            shippingAddress
            ?? throw new DomainException(
                "Shipping address is required.");

        Status =
            OrderStatus.PendingVerification;

        TotalAmount = 0;
    }


    // ============================================================
    // ADD ITEM
    // ============================================================

    public void AddItem(
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

        if (quantity <= 0)
        {
            throw new DomainException(
                "Order item quantity must be greater than zero.");
        }

        if (unitPrice < 0)
        {
            throw new DomainException(
                "Unit price cannot be negative.");
        }

        var existingItem =
            _items.FirstOrDefault(
                x => x.ProductId == productId);

        if (existingItem is not null)
        {
            existingItem.IncreaseQuantity(
                quantity);

            RecalculateTotal();

            return;
        }

        var orderItem =
            new OrderItem(
                this,
                productId,
                productName,
                sku,
                unitPrice,
                quantity);

        _items.Add(orderItem);

        RecalculateTotal();
    }


    // ============================================================
    // VERIFICATION
    // ============================================================

    public void CreateOrRenewVerification(
        string codeHash,
        DateTime expiresAtUtc,
        DateTime utcNow)
    {
        if (Status !=
            OrderStatus.PendingVerification)
        {
            throw new DomainException(
                "Verification can only be created for a pending order.");
        }

        if (Verification is null)
        {
            Verification =
                new OrderVerification(
                    this,
                    codeHash,
                    expiresAtUtc,
                    utcNow);

            return;
        }

        Verification.Renew(
            codeHash,
            expiresAtUtc,
            utcNow);
    }

    public void EnsureVerificationCanBeAttempted(
        DateTime utcNow)
    {
        if (Status !=
            OrderStatus.PendingVerification)
        {
            throw new DomainException(
                "Only a pending order can be verified.");
        }

        if (Verification is null)
        {
            throw new DomainException(
                "Order verification does not exist.");
        }

        Verification.EnsureCanAttempt(
            utcNow);
    }

    public void RegisterFailedVerificationAttempt(
        DateTime utcNow)
    {
        if (Verification is null)
        {
            throw new DomainException(
                "Order verification does not exist.");
        }

        Verification.RegisterFailedAttempt(
            utcNow);
    }

    public void MarkVerificationSuccessful(
        DateTime utcNow)
    {
        if (Verification is null)
        {
            throw new DomainException(
                "Order verification does not exist.");
        }

        Verification.MarkVerified(
            utcNow);
    }


    // ============================================================
    // CONFIRM
    // ============================================================

    public void Confirm(
        DateTime utcNow)
    {
        if (Status !=
            OrderStatus.PendingVerification)
        {
            throw new DomainException(
                "Only pending orders can be confirmed.");
        }

        if (Verification is null ||
            !Verification.IsVerified)
        {
            throw new DomainException(
                "Order must be verified before confirmation.");
        }

        if (_items.Count == 0)
        {
            throw new DomainException(
                "An empty order cannot be confirmed.");
        }

        Status =
            OrderStatus.Confirmed;

        ConfirmedAtUtc =
            utcNow;
    }


    // ============================================================
    // PREPARING
    // ============================================================

    public void StartPreparing()
    {
        if (Status !=
            OrderStatus.Confirmed)
        {
            throw new DomainException(
                "Only confirmed orders can be prepared.");
        }

        Status =
            OrderStatus.Preparing;
    }


    // ============================================================
    // SHIPPED
    // ============================================================

    public void MarkAsShipped(
        DateTime utcNow)
    {
        if (Status !=
            OrderStatus.Preparing)
        {
            throw new DomainException(
                "Only preparing orders can be shipped.");
        }

        Status =
            OrderStatus.Shipped;

        ShippedAtUtc =
            utcNow;
    }


    // ============================================================
    // DELIVERED
    // ============================================================

    public void MarkAsDelivered(
        DateTime utcNow)
    {
        if (Status !=
            OrderStatus.Shipped)
        {
            throw new DomainException(
                "Only shipped orders can be delivered.");
        }

        Status =
            OrderStatus.Delivered;

        DeliveredAtUtc =
            utcNow;
    }


    // ============================================================
    // CANCEL
    // ============================================================

    public void Cancel(
        DateTime utcNow,
        string reason)
    {
        if (Status is
            OrderStatus.Shipped
            or OrderStatus.Delivered
            or OrderStatus.Cancelled)
        {
            throw new DomainException(
                "This order cannot be cancelled.");
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new DomainException(
                "Cancellation reason is required.");
        }

        Status =
            OrderStatus.Cancelled;

        CancelledAtUtc =
            utcNow;

        CancellationReason =
            reason.Trim();
    }


    // TOTAL
    

    private void RecalculateTotal()
    {
        TotalAmount =
            _items.Sum(
                x => x.LineTotal);
    }
}