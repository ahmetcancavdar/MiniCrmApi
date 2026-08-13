using MiniCrm.Domain.Common;
using MiniCrm.Domain.Exceptions;

namespace MiniCrm.Domain.Entities;

public class Product : BaseEntity
{
    public int CategoryId { get; private set; }

    public Category Category { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;

    public string SKU { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public decimal Price { get; private set; }

    public int StockQuantity { get; private set; }

    public string? ImageUrl { get; private set; }

    public bool IsActive { get; private set; } = true;

    public List<StockMovement> StockMovements { get; private set; } = new();

    private Product()
    {
    }

    public Product(
        int categoryId,
        string name,
        string sku,
        string? description,
        decimal price,
        string? imageUrl)
    {
        if (categoryId <= 0)
        {
            throw new DomainException("A valid category is required.");
        }

        CategoryId = categoryId;

        ChangeName(name);
        ChangeSku(sku);
        ChangeDescription(description);
        ChangePrice(price);
        ChangeImageUrl(imageUrl);

        StockQuantity = 0;
        IsActive = true;
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Product name cannot be empty.");
        }

        Name = name.Trim();
    }

    public void ChangeSku(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new DomainException("SKU cannot be empty.");
        }

        SKU = sku.Trim().ToUpperInvariant();
    }

    public void ChangeDescription(string? description)
    {
        Description = description?.Trim();
    }

    public void ChangePrice(decimal price)
    {
        if (price < 0)
        {
            throw new DomainException("Product price cannot be negative.");
        }

        Price = price;
    }

    public void ChangeCategory(int categoryId)
    {
        if (categoryId <= 0)
        {
            throw new DomainException("A valid category is required.");
        }

        CategoryId = categoryId;
    }

    public void ChangeImageUrl(string? imageUrl)
    {
        ImageUrl = imageUrl?.Trim();
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException(
                "Stock increase quantity must be greater than zero.");
        }

        StockQuantity += quantity;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException(
                "Stock decrease quantity must be greater than zero.");
        }

        if (StockQuantity < quantity)
        {
            throw new DomainException(
                "Insufficient product stock.");
        }

        StockQuantity -= quantity;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}