using System.ComponentModel.DataAnnotations;

namespace MiniCrm.Application.DTOs.Addresses;

public class CreateAddressRequestDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } =
        string.Empty;

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

    public bool IsDefault { get; set; }
}