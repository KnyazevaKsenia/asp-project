using AspProject.Domain.Entities;

namespace AspProject.Api.Models;

public class CreateTicketRequest
{
    public List<string> Questions{get;set;}
    public List<string> Tasks{get;set;}
    public TimeOnly Time{get;set;}
}

