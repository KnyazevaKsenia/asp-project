using AspProject.Api.Models;
using AspProject.Domain.Models;

namespace AspProject.Domain.Abstractions;

public interface IMaterialsService
{
    public Task<bool> SaveMaterial(MaterialDto materialDto);
    public Task<List<MaterialResponseDto>?> GetMaterialsByFilters(FiltersDto filtersDto);
    public Task<MaterialResponseDto?> GetMaterialById(Guid id);
    public Task<List<MaterialResponseDto>?> GetMaterialsByKeyWord(string keyword);
    public Task<List<MaterialResponseDto>?> GetMaterialsByStudentId(Guid studentId);
    public Task<bool> DeleteMaterialById(Guid id);
    public Task<bool> ChangeMaterial(AddNewMaterialRequest material, Guid materialId);
}