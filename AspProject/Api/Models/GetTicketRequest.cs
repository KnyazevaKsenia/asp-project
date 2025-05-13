namespace AspProject.Api.Models;

public class GetTicketRequest
{
    public Guid UserId { get; set; }
    public Guid TicketId { get; set; }
}

