using MiniCrm.Domain.Common;
using MiniCrm.Domain.Exceptions;

namespace MiniCrm.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public bool IsActive { get; private set; } = true;

    public List<Product> Products { get; private set; } = new();

    private Category()
    {
    }

    public Category(string name, string? description)
    {
        ChangeName(name);

        Description = description;
        IsActive = true;
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Category name cannot be empty.");
        }

        Name = name.Trim();
    }

    public void ChangeDescription(string? description)
    {
        Description = description?.Trim();
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