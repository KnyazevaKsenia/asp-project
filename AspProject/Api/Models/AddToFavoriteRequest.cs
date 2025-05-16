namespace AspProject.Api.Models;

public class AddToFavoriteRequest
{
    public Guid StudentId { get; set; }
    public Guid MaterialId { get; set; }
}
