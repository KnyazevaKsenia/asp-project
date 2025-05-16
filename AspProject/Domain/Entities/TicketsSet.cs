namespace AspProject.Domain.Entities;

public class TicketsSet
{
    public required Guid Id { get; set; }
    public required string Subject { get; set; }
    public required Guid StudentId { get; set; }
    public  List<Ticket> Tickets { get; set; }
    public TimeOnly Duration { get; set; }
}
