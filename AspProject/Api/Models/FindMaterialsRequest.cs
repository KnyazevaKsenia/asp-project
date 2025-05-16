using AspProject.Domain.Services;

namespace AspProject.Api.Models;

public class FindMaterialsRequest
{
    public Guid StudentId { get; set; }
    public int Course {get; set;}
    public string Subject {get; set;}
    public string TeacherName {get; set;}
    public int Semester {get; set;}
}
