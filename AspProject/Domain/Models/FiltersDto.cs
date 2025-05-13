namespace AspProject.Domain.Models;

public class FiltersDto
{
    public FiltersDto(int course, string? subject, string? teacherName, int semester)
    {
        Course = course;
        Subject = subject;
        TeacherName = teacherName;
        Semester = semester;
    }
    public int Course { get; set; }
    public string? Subject { get; set; }
    public string? TeacherName { get; set; }
    public int Semester { get; set; }
}
