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
        
        examGroup.MapGet("getTicketsSets", async (ITicketService ticketService,
                                                    ClaimsPrincipal student) =>
        {
            var studentId = student.Claims.FirstOrDefault(c => c.Type == "StudentId")?.Value;
            if (studentId != null && Guid.TryParse(studentId, out Guid studentGuidId))
            {
                var result = await ticketService.GetTicketSets(studentGuidId);
                return Results.Json(result);
            }
            return Results.BadRequest();
        }).RequireAuthorization();
        examGroup.MapGet("get-tickets-set/{id}", async (Guid id, ITicketService ticketService) =>
        {
            var set = await ticketService.GetTicketSets(id);
            return Results.Json(set);
            
        }).RequireAuthorization();
        examGroup.MapGet("/delete-ticket-set/{id}", async (Guid id, ITicketService ticketService) =>
        {
            if (await ticketService.DeleteTicketSet(id))
            {
                return Results.Ok();
            }
            return Results.BadRequest();
        }).RequireAuthorization();
        return examGroup;
    }
}