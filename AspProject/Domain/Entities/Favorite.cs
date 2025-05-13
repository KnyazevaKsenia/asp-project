using Microsoft.EntityFrameworkCore;

namespace AspProject.Domain.Entities;

public class Favorite
{
    public Guid StudentId { get; set; }
    public Guid MaterialId { get; set; }
    public Student Student { get; set; }
    public Material Material { get; set; }
}



