namespace AspProject.Domain.Entities;

public class Ticket
{
    public Guid Id { get; set; }
    public string FirstQuestion { get; set; } = string.Empty;
    public string SecondQuestion { get; set; }= string.Empty;
    public Guid TicketsSetId { get; set; }
    public TicketsSet TicketsSet { get; set; } = null!;
}
