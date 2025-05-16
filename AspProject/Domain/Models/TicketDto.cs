namespace AspProject.Domain.Models;

public class TicketDto
{
    public Guid Id { get; set; }
    public string FirstQuestion { get; set; } = string.Empty;
    public string SecondQuestion { get; set; }= string.Empty;
    public Guid TicketsSetId { get; set; }
}
