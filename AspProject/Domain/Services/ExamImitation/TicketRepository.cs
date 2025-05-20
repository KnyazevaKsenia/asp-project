using AspProject.Domain.Abstractions;
using AspProject.Domain.Abstractions.IExamImitation;
using AspProject.Domain.Entities;
using AspProject.Domain.Models;
using AspProject.Infrastrastructure.Database;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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
    
    public async Task<List<TicketSetDto>> GetAllStudentTicketSets(Guid studentId)
    {
        var sets = await context.TicketsSets.Where(x => x.StudentId == studentId).Include(x=>x.Tickets).ToListAsync();
        var setsDtos = mapper.Map<List<TicketSetDto>>(sets);
        return setsDtos;
    }

    public async Task<TicketSetDto> GetTicketSetById(Guid id)
    {
        var set = await context.TicketsSets.Where(x => x.Id == id).FirstOrDefaultAsync();
        return mapper.Map<TicketSetDto>(set);
    }

    public async Task<bool> DeleteTicketSet(Guid id)
    {
        
        var tickets = await context.TicketsSets.Where(x => x.Id == id).Include(x=> x.Tickets).FirstOrDefaultAsync();
        try
        {
            context.TicketsSets.RemoveRange(tickets);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    
}