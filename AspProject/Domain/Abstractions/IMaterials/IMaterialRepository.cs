﻿using AspProject.Domain.Models;

namespace AspProject.Domain.Abstractions;

public interface IMaterialRepository
{
    public Task AddMaterial(MaterialDto dto);
    public Task<List<MaterialResponseDto>?> GetMaterialsByFilters(FiltersDto filters);
    public Task<MaterialResponseDto?> GetMaterialById(Guid id);
    public Task<List<MaterialResponseDto>?> GetMaterialsByKeyWords(string keyword);
    public Task<List<MaterialResponseDto>?> GetMaterialsByStudent(Guid studentId);
    public Task<bool> DeleteMaterialById(Guid id);
    public Task<bool> ChangeMaterial(MaterialDto material);
}