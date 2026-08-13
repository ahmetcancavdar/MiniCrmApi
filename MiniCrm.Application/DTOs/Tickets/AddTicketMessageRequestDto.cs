using System.ComponentModel.DataAnnotations;

namespace MiniCrm.Application.DTOs.Tickets;

public class AddTicketMessageRequestDto
{
    [Required]
    [MaxLength(4000)]
    public string Message { get; set; } =
        string.Empty;
}