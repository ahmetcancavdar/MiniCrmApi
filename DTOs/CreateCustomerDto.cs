using System.ComponentModel.DataAnnotations;

namespace MiniCrmApi.DTOs;

public class CreateCustomerDto
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string? CompanyName { get; set; }
}