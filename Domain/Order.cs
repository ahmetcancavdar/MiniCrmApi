namespace MiniCrmApi.Domain;

public class Order : BaseEntity
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public decimal TotalPrice { get; private set; }

    public List<OrderItem> OrderItems { get; set; } = new();

    public void CalculateTotalPrice()
    {
        TotalPrice = OrderItems.Sum(x => x.LineTotal);
    }
}