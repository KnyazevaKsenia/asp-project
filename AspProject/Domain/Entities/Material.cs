using System.Security.Cryptography.X509Certificates;
using AspProject.Domain.Services;

namespace AspProject.Domain.Entities;

public class Material
{
    public Guid MaterialId { get; set; }
    public Guid StudentId { get; set; }
    public int Course { get; set; }
    public string Subject { get; set; }
    public string TeacherName { get; set; }
    public int Semester { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public List<Favorite> Favorites { get; set; } = new();
}







