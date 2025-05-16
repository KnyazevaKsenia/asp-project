using AspProject.Domain.Entities;

namespace AspProject.Api.Models;

public class CreateTicketRequest
{
    public string SubjectName { get; set; }
    public List<string> Questions{get;set;}
    public TimeOnly Duration{get;set;}
}

