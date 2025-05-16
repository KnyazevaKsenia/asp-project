using AspProject.Domain.Abstractions;
using AspProject.Domain.Entities;
using AspProject.Domain.Models;
using AspProject.Infrastrastructure.Database;
using AutoMapper;

namespace AspProject.Domain.Services.ExamImitation;

public class TicketRepository(AppDbContext context, IMapper mapper) : ITicketRepository
{
    public async Task<bool> SaveTicketSet(TicketSetDto setDto,Guid studentId)
    {
        var tickets = mapper.Map<List<Ticket>>(setDto.Tickets);
        var set = new TicketsSet()
        {
            Id = setDto.Id,
            Duration = setDto.Duration,
            StudentId = studentId,
            Subject = setDto.Subject,
            Tickets = tickets
        };
        
        set.StudentId = studentId;
        try
        {
            await context.TicketsSets.AddAsync(set);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}