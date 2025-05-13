using System.Security.Cryptography.X509Certificates;
using AspProject.Api.Models;
using AspProject.Domain.Abstractions;
using AspProject.Domain.Models;
using AspProject.Domain.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AspProject.Api.Endpoints;

public static class MaterialsEndpointExt
{
    public static IEndpointRouteBuilder MapMaterials(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("materials/");
        group.MapGet("get-materials/", async  (int course,string? subject, string? teacherName, int semester, IFileService fileService, IMaterialsService materialsService) =>
        {
            var filters = new FiltersDto(course, subject, teacherName, semester);
            List<MaterialResponse> response = new List<MaterialResponse>();
            var materials = await materialsService.GetMaterialsByFilters(filters);
            foreach (var material in materials)
            {
                var file = fileService.GetMaterialFiles(material.MaterialId);
                response.Add(new MaterialResponse
                {
                    Id = material.MaterialId,
                    Description = material.Description,
                    Course = material.Course,
                    Subject = material.Subject,
                    Teacher = material.TeacherName,
                    Semester = material.Semester,
                    CreatedAt = material.Date,
                    ImagesNameUrl = file?.ImagesNameUrl ?? new Dictionary<string, string>(),
                    FileNameUrl = file?.FileNameUrl ?? new Dictionary<string, string>()
                });
            }
            return Results.Json(response);
        });
        group.MapGet("/file/{materialId}/{name}", async (string name,Guid materialId, IFileService fileService) =>
        {
            var testString = name;
            Console.WriteLine(testString);
            var result = await fileService.ReturnFilesByPath(name, materialId);
            if (result == null)
            {
                return Results.NotFound();
            }
            else
            {
                return Results.File(result.Item1, result.Item2);
            }
        });
        
        group.MapGet("get-material/{materialId}",async  (Guid materialId, IMaterialsService materialsService, IFileService fileService) =>
        {
            var material = await  materialsService.GetMaterialById(materialId);
            var files = fileService.GetMaterialFiles(materialId);
            if (material != null)
            {
                var result = new MaterialResponse
                {
                    Id = material.MaterialId,
                    Description = material.Description,
                    Course = material.Course,
                    Subject = material.Subject,
                    Teacher = material.TeacherName,
                    Semester = material.Semester,
                    CreatedAt = material.Date,
                    ImagesNameUrl = files?.ImagesNameUrl ?? new Dictionary<string, string>(),
                    FileNameUrl = files?.FileNameUrl ?? new Dictionary<string, string>()
                };
                return Results.Json(result);
            }
            else
            {
                return Results.NotFound();
            }
        });
        group.MapGet("find-by-keyword/{keyword}", async (string keyword, IMaterialsService materialsService, IFileService fileService, IMapper mapper) =>
        {
            var collection = mapper.Map<List<MaterialResponseDto>, List<MaterialResponse>>(await materialsService.GetMaterialsByKeyWord(keyword));
            
            if (collection != null)
            {
                foreach (var material in collection)
                {
                    var file = fileService.GetMaterialFiles(material.Id);
                    material.ImagesNameUrl = file?.ImagesNameUrl ?? new Dictionary<string, string>();
                    material.FileNameUrl = file?.FileNameUrl ?? new Dictionary<string, string>();   
                }
                return Results.Json(collection);
            }
            else
            {
                return Results.NotFound();
            }
        });
        return group;
    }
}
