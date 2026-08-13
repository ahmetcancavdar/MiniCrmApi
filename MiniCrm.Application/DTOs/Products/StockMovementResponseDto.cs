namespace MiniCrm.Application.DTOs.Products;

public class StockMovementResponseDto
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string MovementType { get; set; } = string.Empty;

    public int QuantityChange { get; set; }

    public int PreviousQuantity { get; set; }

    public int NewQuantity { get; set; }

    public string? Note { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}