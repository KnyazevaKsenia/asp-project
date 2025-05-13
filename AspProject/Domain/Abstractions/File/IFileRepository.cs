using AspProject.Domain.Entities;
using AspProject.Domain.Models;

namespace AspProject.Domain.Abstractions;

public interface IFileRepository
{
    public Task<bool> AddFiles(IFormFileCollection collection, Guid materialId);
    public MaterialFilesDto GetFilesToMaterial(Guid materialId);
    public Task<Tuple<byte[], string>> ReturnFileByPath(string fileName, Guid materialId);
}