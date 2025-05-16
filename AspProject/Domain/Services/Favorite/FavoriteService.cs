using AspProject.Api.Models;
using AspProject.Domain.Abstractions;
using AspProject.Infrastrastructure.Database;
using AutoMapper;

namespace AspProject.Domain.Services;

public class FavoriteService(AppDbContext context, IMapper mapper, IFavoriteRepository favoriteRepository, IFileService fileService) :IFavoriteService
{
    public async Task<bool> AddFavorite(Guid materialId, Guid studentId)
    {
        return await favoriteRepository.AddFavorite(materialId, studentId);
    }
    
    public async Task<bool> RemoveFavorite(Guid userId,Guid materialId)
    {
        return await favoriteRepository.RemoveFavorite(userId,materialId);
    }
    public async Task<List<MaterialResponse>> GetStudentFavorites(Guid sturdentId)
    {
        var favorites = await favoriteRepository.GetStudentFavorites(sturdentId);
        var result = new List<MaterialResponse>();
        foreach (var favorite in favorites)
        {
            var file = fileService.GetMaterialFiles(favorite.MaterialId);
            result.Add(new MaterialResponse
            {
                Id = favorite.MaterialId,
                Description = favorite.Description,
                Course = favorite.Course,
                Subject = favorite.Subject,
                Teacher = favorite.TeacherName,
                Semester = favorite.Semester,
                CreatedAt = favorite.Date,
                ImagesNameUrl = file?.ImagesNameUrl ?? new Dictionary<string, string>(),
                FileNameUrl = file?.FileNameUrl ?? new Dictionary<string, string>()
            });
        }
        return result;
    }

    public async Task<List<Guid>> GetStudentFavoritesIds(Guid sturdentId)
    {
        return await favoriteRepository.GetStudentFavoriteIds(sturdentId);
    }
}