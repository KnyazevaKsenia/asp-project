namespace AspProject.Domain.Models;

public class TicketSetDto
{
    
    public required Guid Id { get; set; }
    public required string Subject { get; set; }
    public required List<TicketDto> Tickets { get; set; }
    public required TimeOnly Duration { get; set; }
}
