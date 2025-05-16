using AspProject.Api.Models;

namespace AspProject.Domain.Abstractions;

public interface IFavoriteService
{
    public Task<bool> AddFavorite(Guid materialId, Guid studentId);
    public Task<bool> RemoveFavorite(Guid userId,Guid materialId);
    public Task<List<MaterialResponse>> GetStudentFavorites(Guid sturdentId);
    public Task<List<Guid>> GetStudentFavoritesIds(Guid sturdentId);
}