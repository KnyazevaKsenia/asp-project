using AspProject.Api.Models;
using AspProject.Domain.Models;

namespace AspProject.Domain.Abstractions;

public interface ITicketService
{
    public Task<bool> CreateTicketsSet(CreateTicketRequest createTicketRequest, Guid studentId);
    public Task<List<TicketSetDto>> GetTicketSets(Guid studentId);
    public Task<TicketSetDto> GetTicketSetById(Guid ticketSetId);
    public Task<bool> DeleteTicketSet(Guid ticketSetId);
}

