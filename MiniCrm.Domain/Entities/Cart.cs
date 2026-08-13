using MiniCrm.Domain.Common;
using MiniCrm.Domain.Exceptions;

namespace MiniCrm.Domain.Entities;

public class Cart : BaseEntity
{
    private readonly List<CartItem> _items = new();

    public int CustomerId { get; private set; }

    public Customer Customer { get; private set; } = null!;

    public IReadOnlyCollection<CartItem> Items =>
        _items.AsReadOnly();

    private Cart()
    {
    }

    public Cart(int customerId)
    {
        if (customerId <= 0)
        {
            throw new DomainException(
                "A valid customer is required.");
        }

        CustomerId = customerId;
    }

    public void AddProduct(
        int productId,
        int quantity)
    {
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

        var existingItem =
            _items.FirstOrDefault(x =>
                x.ProductId == productId &&
                !x.IsDeleted);

        if (existingItem is not null)
        {
            existingItem.IncreaseQuantity(
                quantity);

            return;
        }

        var cartItem =
            new CartItem(
                this,
                productId,
                quantity);

        _items.Add(cartItem);
    }

    public void ChangeProductQuantity(
        int productId,
        int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException(
                "Product quantity must be greater than zero.");
        }

        var item =
            FindActiveItem(productId);

        item.ChangeQuantity(quantity);
    }

    public void RemoveProduct(
        int productId)
    {
        var item =
            FindActiveItem(productId);

        item.MarkDeleted();
    }

    public void Clear()
    {
        foreach (var item in
                 _items.Where(x => !x.IsDeleted))
        {
            item.MarkDeleted();
        }
    }

    private CartItem FindActiveItem(
        int productId)
    {
        var item =
            _items.FirstOrDefault(x =>
                x.ProductId == productId &&
                !x.IsDeleted);

        if (item is null)
        {
            throw new DomainException(
                "Product does not exist in the cart.");
        }

        return item;
    }
}