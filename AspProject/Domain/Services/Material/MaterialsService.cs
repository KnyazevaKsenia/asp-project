using AspProject.Api.Models;
using AspProject.Domain.Abstractions;
using AspProject.Domain.Models;
using AutoMapper;

namespace AspProject.Domain.Services;

public class MaterialsService(IMaterialRepository materialRepository, IFileService fileService) : IMaterialsService
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
    
    public async Task<List<MaterialResponseDto>?> GetMaterialsByKeyWord(string keyword)
    {
        var result = await materialRepository.GetMaterialsByKeyWords(keyword);
        return result;
    }
}

