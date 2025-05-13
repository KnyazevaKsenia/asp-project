namespace AspProject.Domain.Models;

public class MaterialResponseDto
{
    public Guid UserId { get; set; }
    public Guid MaterialId { get; set; }
    public string Description { get; set; }
    public int Course  { get; set; }
    public string Subject { get; set; }
    public string TeacherName { get; set; }
    public int Semester { get; set; }
    public DateTime Date { get; set; }
}

