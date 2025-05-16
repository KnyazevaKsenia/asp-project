using AspProject.Api.Models;
using AspProject.Domain.Abstractions;
using AspProject.Domain.Models;
using AutoMapper;

namespace AspProject.Domain.Services;

public class MaterialsService(IMaterialRepository materialRepository, IFileService fileService, IMapper mapper) : IMaterialsService
{
    public async Task<bool> SaveMaterial(MaterialDto materialDto)
    {
        try
        {   
            await materialRepository.AddMaterial(materialDto);
            return true;
        }
        catch(Exception e)
        {
            return false;
        }
    }
    
    public async Task<List<MaterialResponseDto>?> GetMaterialsByFilters(FiltersDto filtersDto)
    {
        var result = await materialRepository.GetMaterialsByFilters(filtersDto);
        return result;
    }
    
    public async Task<MaterialResponseDto?> GetMaterialById(Guid id)
    {
        var material = await materialRepository.GetMaterialById(id);
        return material;
    }
    
    public async Task<List<MaterialResponseDto>?> GetMaterialsByStudentId(Guid studentId)
    {
        return await materialRepository.GetMaterialsByStudent(studentId);
    }
    
    public async Task<bool> DeleteMaterialById(Guid id)
    {
        return await materialRepository.DeleteMaterialById(id);
    }
    
    public async Task<bool> ChangeMaterial(AddNewMaterialRequest material, Guid materialId)
    {
        var materialDto = mapper.Map<AddNewMaterialRequest,MaterialDto>(material);
        materialDto.MaterialId = materialId;
        var result = await materialRepository.ChangeMaterial(materialDto);
        return result;
    }
    
    public async Task<List<MaterialResponseDto>?> GetMaterialsByKeyWord(string keyword)
    {
        var result = await materialRepository.GetMaterialsByKeyWords(keyword);
        return result;
    }
}

