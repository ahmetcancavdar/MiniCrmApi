using MiniCrm.Domain.Exceptions;

namespace MiniCrm.Domain.ValueObjects;

public sealed class OrderAddress
{
    public string RecipientName { get; private set; } = string.Empty;

    public string? Phone { get; private set; }

    public string AddressLine { get; private set; } = string.Empty;

    public string City { get; private set; } = string.Empty;

    public string District { get; private set; } = string.Empty;

    public string? PostalCode { get; private set; }

    public string Country { get; private set; } = string.Empty;

    private OrderAddress()
    {
    }

    public OrderAddress(
        string recipientName,
        string? phone,
        string addressLine,
        string city,
        string district,
        string? postalCode,
        string country)
    {
        if (string.IsNullOrWhiteSpace(recipientName))
        {
            throw new DomainException(
                "Recipient name cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(addressLine))
        {
            throw new DomainException(
                "Address line cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(city))
        {
            throw new DomainException(
                "City cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(district))
        {
            throw new DomainException(
                "District cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(country))
        {
            throw new DomainException(
                "Country cannot be empty.");
        }

        RecipientName = recipientName.Trim();
        Phone = phone?.Trim();
        AddressLine = addressLine.Trim();
        City = city.Trim();
        District = district.Trim();
        PostalCode = postalCode?.Trim();
        Country = country.Trim();
    }
}