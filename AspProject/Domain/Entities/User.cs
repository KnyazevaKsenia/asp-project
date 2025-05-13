namespace AspProject.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Login { get; set; } 
    public string Password { get; set; } 
    public Student? Student { get; set; }
}
