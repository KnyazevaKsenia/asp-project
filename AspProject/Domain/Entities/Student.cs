namespace AspProject.Domain.Entities;

public class Student
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FirstName { get; set; } 
    public string LastName { get; set; }
    public string University { get; set; } 
    public string Institute { get; set; } 
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public List<Favorite> Favorites { get; set; } = new();
}



