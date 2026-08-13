namespace MiniCrm.Application.DTOs.Tickets;

public class AdminTicketDetailResponseDto
{
	public int CustomerId { get; set; }

	public string CustomerName { get; set; } =
		string.Empty;

	public string CustomerEmail { get; set; } =
		string.Empty;

	public TicketDetailResponseDto Ticket { get; set; } =
		new();
}