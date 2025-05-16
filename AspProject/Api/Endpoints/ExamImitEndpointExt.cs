using System.Security.Claims;
using AspProject.Api.Models;
using AspProject.Domain.Abstractions;

namespace AspProject.Api.Endpoints;

public static class ExamImitEndpointExt
{
    public static IEndpointRouteBuilder MapExamImitEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var examGroup = endpoints.MapGroup("examImitation");
        examGroup.MapPost("/createTicketSet", async (CreateTicketRequest? createRequest,
                                                    ClaimsPrincipal student, 
                                                    ITicketService ticketService) =>
        {
            var studentId = student.Claims.FirstOrDefault(c => c.Type == "StudentId")?.Value;
            if (studentId != null && Guid.TryParse(studentId, out Guid studentGuidId) && createRequest!=null)
            {
                if (await ticketService.CreateTicketsSet(createRequest, studentGuidId))
                {
                    return Results.Ok();
                }
                return Results.Problem("Problem adding tickets set");
            }
            return Results.Unauthorized();
            
        }).RequireAuthorization();
        examGroup.MapGet("getTicketsSets", async () =>
        {
            
        });
        
        return examGroup;
    }
}