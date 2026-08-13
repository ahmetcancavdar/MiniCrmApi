using System.ComponentModel.DataAnnotations;
using MiniCrm.Domain.Enums;

namespace MiniCrm.Application.DTOs.Tickets;

public class CreateTicketRequestDto
{
    [Required]
    [MaxLength(200)]
    public string Subject { get; set; } =
        string.Empty;

    public TicketPriority Priority { get; set; }

    public int? OrderId { get; set; }

    [Required]
    [MaxLength(4000)]
    public string Message { get; set; } =
        string.Empty;
}