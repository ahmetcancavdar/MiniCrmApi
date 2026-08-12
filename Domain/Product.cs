namespace MiniCrmApi.Domain;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new();
}