using MiniCrm.Domain.Common;
using MiniCrm.Domain.Exceptions;

namespace MiniCrm.Domain.Entities;

public class CartItem : BaseEntity
{
    public int CartId { get; private set; }

    public Cart Cart { get; private set; } = null!;

    public int ProductId { get; private set; }

    public Product Product { get; private set; } = null!;

    public int Quantity { get; private set; }

    private CartItem()
    {
    }

    internal CartItem(
        Cart cart,
        int productId,
        int quantity)
    {
        if (cart is null)
        {
            throw new DomainException(
                "Cart is required.");
        }

        if (productId <= 0)
        {
            throw new DomainException(
                "A valid product is required.");
        }

        if (quantity <= 0)
        {
            throw new DomainException(
                "Product quantity must be greater than zero.");
        }

        Cart = cart;
        CartId = cart.Id;

        ProductId = productId;
        Quantity = quantity;
    }

    internal void IncreaseQuantity(
        int quantity)
    {
        if (IsDeleted)
        {
            throw new DomainException(
                "A deleted cart item cannot be changed.");
        }

        if (quantity <= 0)
        {
            throw new DomainException(
                "Quantity increase must be greater than zero.");
        }

        Quantity += quantity;
    }

    internal void ChangeQuantity(
        int quantity)
    {
        if (IsDeleted)
        {
            throw new DomainException(
                "A deleted cart item cannot be changed.");
        }

        if (quantity <= 0)
        {
            throw new DomainException(
                "Product quantity must be greater than zero.");
        }

        Quantity = quantity;
    }

    internal void MarkDeleted()
    {
        if (IsDeleted)
        {
            return;
        }

        IsDeleted = true;
    }
}