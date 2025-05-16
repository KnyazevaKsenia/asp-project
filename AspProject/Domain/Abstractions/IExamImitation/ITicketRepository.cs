using AspProject.Domain.Models;

namespace AspProject.Domain.Abstractions;

public interface ITicketRepository
{
    public Task<bool> SaveTicketSet(TicketSetDto ticket, Guid userId);
}