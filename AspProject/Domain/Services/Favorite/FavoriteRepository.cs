using AspProject.Api.Models;
using AspProject.Domain.Abstractions;
using AspProject.Domain.Entities;
using AspProject.Domain.Models;
using AspProject.Infrastrastructure.Database;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AspProject.Domain.Services;

public class FavoriteRepository(AppDbContext context, IMapper mapper) : IFavoriteRepository
{
    public async Task<bool> AddFavorite(Guid materialId, Guid studentId)
    {
        bool materialExists = await context.Materials
            .AnyAsync(m => m.MaterialId == materialId);
        bool studentExists = await context.Students
            .AnyAsync(s => s.Id == studentId);
        
        if (!materialExists || !studentExists)
        {
            return false; 
        }
        
        var favorite = new Favorite
        {
            MaterialId = materialId,
            StudentId = studentId
        };
        try
        {
            await context.Favorites.AddAsync(favorite);
            await context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RemoveFavorite(Guid userId,Guid materialId)
    {
        var favorite = await context.Favorites.FindAsync(userId,materialId);
        if (favorite == null)
        {
            return false;
        }
        context.Favorites.Remove(favorite);
        await context.SaveChangesAsync();
        return true;
    }
    
    public async Task<List<MaterialResponseDto>> GetStudentFavorites(Guid sturdentId)
    {
        var materialsIds = await context.Favorites.Where(f => f.StudentId == sturdentId).Select(f => f.MaterialId).ToListAsync();
        var materials = new List<Material>();
        foreach (var id in materialsIds )
        {
            materials.Add(await context.Materials.FindAsync(id));
        }
        return mapper.Map<List<MaterialResponseDto>>(materials);
    }

    public async Task<List<Guid>> GetStudentFavoriteIds(Guid sturdentId)
    {
        var materialsIds = await context.Favorites.Where(f => f.StudentId == sturdentId).Select(f => f.MaterialId).ToListAsync();
        return materialsIds;
    }
}