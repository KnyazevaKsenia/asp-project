using AspProject.Domain.Abstractions;
using AspProject.Domain.Entities;
using AspProject.Domain.Models;
using Microsoft.AspNetCore.StaticFiles;

namespace AspProject.Domain.Services;

public class FileRepository : IFileRepository
{
    public async Task<bool> AddFiles(IFormFileCollection collection, Guid materialId)
    {
        try
        {
            var projectRoot = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..");
            var uploadPath = Path.Combine(projectRoot, "FilesRepository", materialId.ToString());
            
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);
            
            foreach (var file in collection)
            {
                var filePath = Path.Combine(uploadPath, file.FileName);
                await using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
            }
            
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
            return false;
        }
    }
    
    public MaterialFilesDto GetFilesToMaterial(Guid materialId)
    {
        var projectRoot = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..");
        var downloadPath = Path.Combine(projectRoot, "FilesRepository", materialId.ToString());
        var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
        
        var images = new Dictionary<string, string>();
        var files = new Dictionary<string, string>();
        
        if (!Directory.Exists(downloadPath))
            return null;
        
        foreach (var filePath in Directory.GetFiles(downloadPath))
        {
            var fileName = Path.GetFileName(filePath);
            var extension = Path.GetExtension(filePath).ToLower();
            var url = $"https://localhost:44356/materials/file/{materialId}/{fileName}";
            
            if (imageExtensions.Contains(extension))
                images[fileName] = url;
            else
                files[fileName] = url;
        }
            
        return new MaterialFilesDto
        {
            ImagesNameUrl = images,
            FileNameUrl = files
        };
    }

    public async Task<Tuple<byte[], string>> ReturnFileByPath(string fileName, Guid materialId)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "FilesRepository", materialId.ToString(), fileName);
        
        if (!System.IO.File.Exists(filePath))
        {
            return null;
        }
        
        var file = await File.ReadAllBytesAsync(filePath);
        
        var provider = new FileExtensionContentTypeProvider();
        string contentType;
        
        if (!provider.TryGetContentType(filePath, out contentType))
        {
            contentType = "application/octet-stream"; // если тип не определён
        }
        
        return new Tuple<byte[], string>(file, contentType);
    }
    
}