using AspProject.Domain.Abstractions;
using AspProject.Domain.Entities;
using AspProject.Domain.Models;
using AspProject.Infrastrastructure.Database;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AspProject.Domain.Services;

public class MaterialRepository(AppDbContext context, IMapper mapper) : IMaterialRepository
{
    public async Task AddMaterial(MaterialDto dto)
    {   
        //не маппится правильно studentId
        var material = mapper.Map<Material>(dto);
        context.Materials.Add(material);
        await context.SaveChangesAsync();
    }
    
    public async Task<List<MaterialResponseDto>?> GetMaterialsByFilters(FiltersDto filters)
    {   
        int course = filters.Course;
        string? subject = filters.Subject ;
        string? teacher = filters.TeacherName;
        int semester = filters.Semester;
        
        if (!string.IsNullOrEmpty(teacher) && string.IsNullOrEmpty(subject))
        {
            return null;
        }
        
        if (!string.IsNullOrEmpty(subject) && course == 0)
        {
            return null;
        }

        var query = context.Materials.AsQueryable();
        
        if (course != 0)
        {
            query = query.Where(m => m.Course == course);
        }
            
        if (!string.IsNullOrEmpty(subject))
        {
            query = query.Where(m => m.Subject == subject);

            if (!string.IsNullOrEmpty(teacher))
            {
                query = query.Where(m => m.TeacherName == teacher);
            }
        }

        if (semester != 0)
        {
            query = query.Where(m => m.Semester == semester);
        }
        
        var filteredMaterials = await query.ToListAsync();
        return mapper.Map<List<MaterialResponseDto>>(filteredMaterials);
    }
    
    public async Task<MaterialResponseDto?> GetMaterialById(Guid id)
    {   
        var material = mapper.Map<MaterialResponseDto>(await context.Materials
            .FirstOrDefaultAsync(m => m.MaterialId == id));
        Console.WriteLine(material.MaterialId);
        return material;
    }
    
    public async Task<List<MaterialResponseDto>?> GetMaterialsByStudent(Guid studentId)
    {
        var materials = await context.Materials
            .Where(m => m.StudentId == studentId).AsNoTracking().ToListAsync();
        return mapper.Map<List<MaterialResponseDto>>(materials);
    }
    public async Task<List<MaterialResponseDto>?> GetMaterialsByKeyWords(string keyword)
    {
        var materials = await context.Materials
            .Where(m =>
                EF.Functions.ILike(m.TeacherName, $"%{keyword}%") ||
                EF.Functions.ILike(m.Subject, $"%{keyword}%") ||
                EF.Functions.ILike(m.Description, $"%{keyword}%"))
            .ToListAsync();

        var result = mapper.Map<List<MaterialResponseDto>>(materials);
        return result;
    }
    
    public async Task<bool> ChangeMaterial(MaterialDto material)
    {
        var existingMaterial = await context.Materials
            .FirstOrDefaultAsync(m => m.MaterialId == material.MaterialId);
        
        if (existingMaterial == null)
            return false;
        
        existingMaterial.Course = material.Course;
        existingMaterial.Subject = material.Subject;
        existingMaterial.TeacherName = material.TeacherName;
        existingMaterial.Semester = material.Semester;
        existingMaterial.Description = material.Description;
        existingMaterial.Date = DateTime.UtcNow; 
        
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteMaterialById(Guid id)
    {
        var material = await context.Materials.FirstOrDefaultAsync(m => m.MaterialId == id);
        if (material == null)
        {
            return false;
        }
        context.Materials.RemoveRange(material);
        await context.SaveChangesAsync();
        return true;
    }
}

