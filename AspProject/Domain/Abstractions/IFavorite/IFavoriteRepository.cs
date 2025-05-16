using AspProject.Domain.Models;

namespace AspProject.Domain.Abstractions;

public interface IFavoriteRepository
{
    public Task<bool> AddFavorite(Guid materialId, Guid studentId);
    public Task<bool> RemoveFavorite(Guid userId,Guid materialId);
    public Task<List<MaterialResponseDto>> GetStudentFavorites(Guid sturdentId);
    public Task<List<Guid>> GetStudentFavoriteIds(Guid sturdentId);
}
