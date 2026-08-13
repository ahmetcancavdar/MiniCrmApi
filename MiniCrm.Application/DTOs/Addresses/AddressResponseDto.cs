namespace MiniCrm.Application.DTOs.Addresses;

public class AddressResponseDto
{
    public int Id { get; set; }

    public string Title { get; set; } =
        string.Empty;

    public string AddressLine { get; set; } =
        string.Empty;

    public string City { get; set; } =
        string.Empty;

    public string District { get; set; } =
        string.Empty;

    public string? PostalCode { get; set; }

    public string Country { get; set; } =
        string.Empty;

    public bool IsDefault { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}