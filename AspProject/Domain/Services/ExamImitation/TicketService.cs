using AspProject.Api.Models;
using AspProject.Domain.Abstractions;
using AspProject.Domain.Abstractions.IExamImitation;
using AspProject.Domain.Models;

namespace AspProject.Domain.Services.ExamImitation;

public class TicketService(ITicketRepository ticketRepository): ITicketService
{
    public async Task<bool> CreateTicketsSet(CreateTicketRequest createTicketRequest, Guid studentId)
    {
        var questions = createTicketRequest.Questions;
        var tickets = new List<TicketDto>();
        var set = new TicketSetDto()
        {
            Id = Guid.NewGuid(),
            Duration = createTicketRequest.Duration,
            Subject = createTicketRequest.SubjectName,
            Tickets = tickets,
        };
        int mid = questions.Count / 2;
        int pairCount = Math.Min(mid, questions.Count - mid); 

        for (int i = 0; i < pairCount; i++)
        {
            tickets.Add(new TicketDto
            {
                Id = Guid.NewGuid(),
                FirstQuestion = questions[i],
                SecondQuestion = questions[i + mid],
                TicketsSetId = set.Id,
            });
        }
        
        if (questions.Count % 2 != 0)
        {
            tickets.Add(new TicketDto
            {
                Id = Guid.NewGuid(),
                FirstQuestion = questions[^1],
                SecondQuestion = "",
                TicketsSetId = set.Id,
            });
        }
        
        return await ticketRepository.SaveTicketSet(set, studentId);
    }
    
    public async Task<List<TicketSetDto>> GetTicketSets(Guid studentId)
    {
        var result = await ticketRepository.GetAllStudentTicketSets(studentId);
        return result;
    }
    
    public async Task<TicketSetDto> GetTicketSetById(Guid ticketSetId)
    {
        var result = await ticketRepository.GetTicketSetById(ticketSetId);
        return result;
    }
    
    public async Task<bool> DeleteTicketSet(Guid ticketSetId)
    {
        return await ticketRepository.DeleteTicketSet(ticketSetId);
    }
}