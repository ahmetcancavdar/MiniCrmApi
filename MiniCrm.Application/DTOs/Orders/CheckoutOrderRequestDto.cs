using System.ComponentModel.DataAnnotations;

namespace MiniCrm.Application.DTOs.Orders;

public class CheckoutOrderRequestDto
{
    [Required]
    [MaxLength(150)]
    public string RecipientName { get; set; } =
        string.Empty;

    [MaxLength(30)]
    public string? Phone { get; set; }

    [Required]
    [MaxLength(500)]
    public string AddressLine { get; set; } =
        string.Empty;

    [Required]
    [MaxLength(100)]
    public string City { get; set; } =
        string.Empty;

    [Required]
    [MaxLength(100)]
    public string District { get; set; } =
        string.Empty;

    [MaxLength(20)]
    public string? PostalCode { get; set; }

    [Required]
    [MaxLength(100)]
    public string Country { get; set; } =
        string.Empty;
}