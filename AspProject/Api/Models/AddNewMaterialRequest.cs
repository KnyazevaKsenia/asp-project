using AspProject.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspProject.Api.Models;

public class AddNewMaterialRequest
{   
    
    public Guid StudentId { get; set; }
    public int Course {get; set;}
    public string? Subject {get; set;}
    public string? TeacherName {get; set;}
    public int Semester {get; set;}
    public string? Description {get; set;}
}




