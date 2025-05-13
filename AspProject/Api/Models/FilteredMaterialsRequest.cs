namespace AspProject.Api.Models;

public class FilteredMaterialsRequest
{
    public int Course { get; set; }
    public string? Subject { get; set; }
    public string? TeacherName { get; set; }
    public int Semester { get; set; }
}
