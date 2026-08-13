using System.ComponentModel.DataAnnotations;

namespace MiniCrm.Application.DTOs.Orders;

public class VerifyOrderRequestDto
{
    [Required]
    [RegularExpression(
        @"^\d{6}$",
        ErrorMessage =
            "Verification code must contain exactly 6 digits.")]
    public string Code { get; set; } =
        string.Empty;
}