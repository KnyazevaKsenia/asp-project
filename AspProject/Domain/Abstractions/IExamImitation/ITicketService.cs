using AspProject.Api.Models;

namespace AspProject.Domain.Abstractions;

public interface ITicketService
{
    public Task<bool> CreateTicketsSet(CreateTicketRequest createTicketRequest, Guid studentId);
}

