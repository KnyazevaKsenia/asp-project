namespace AspProject.Api.Models;

public class AddToFavoriteRequest
{
    public Guid UserId { get; set; }
    public Guid MaterialId { get; set; }
}
