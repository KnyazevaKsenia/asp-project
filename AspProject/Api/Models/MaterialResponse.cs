namespace AspProject.Api.Models;

public class MaterialResponse
{
    public required Guid Id { get; set; }
    public string Description { get; set; }
    public required int Course  { get; set; }
    public string Subject { get; set; }
    public string Teacher { get; set; }
    public int Semester { get; set; }
    public required DateTime CreatedAt { get; set; }
    public Dictionary<string, string> ImagesNameUrl { get; set; }
    public Dictionary<string, string> FileNameUrl { get; set; }
}
