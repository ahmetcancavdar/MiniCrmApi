using System.ComponentModel.DataAnnotations;

namespace MiniCrm.Application.DTOs.AfterSales;

public class AfterSalesDecisionRequestDto
{
    [Required]
    [MaxLength(2000)]
    public string AdminNote { get; set; } =
        string.Empty;
}