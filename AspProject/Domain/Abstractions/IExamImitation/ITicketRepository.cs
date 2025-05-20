using AspProject.Domain.Models;

namespace AspProject.Domain.Abstractions.IExamImitation;

public interface ITicketRepository
{
    public Task<bool> SaveTicketSet(TicketSetDto ticket, Guid userId);
    public Task<List<TicketSetDto>> GetAllStudentTicketSets(Guid studentId);
    public Task<TicketSetDto> GetTicketSetById(Guid id);
    public Task<bool> DeleteTicketSet(Guid id);
}