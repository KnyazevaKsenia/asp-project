using AspProject.Api.Models;
using AspProject.Domain.Abstractions;
using AspProject.Domain.Services;
using AspProject.Domain.Abstractions;
using AspProject.Domain.Models;
using AutoMapper;
using System.Linq;
using System.Security.Claims;
using AspProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AspProject.Api.Endpoints;

public static class MaterialEndpointExt
{
    public static IEndpointRouteBuilder MapAddMaterialEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var materialsGroup = endpoints.MapGroup("adding");
        materialsGroup.MapGet("/selection-list", async (ClaimsPrincipal student) =>
        {
            var institute = student.Claims.FirstOrDefault(c => c.Type == "Institute")?.Value;
            if (institute == null)
            {
                return Results.BadRequest("Ошибка при аутентификации, попробуйте позже");
            }
            switch (institute)
            {
                case "Итис":
                    var filePath = @"D:\ASP PROJECT\Backend\AspProject\selectionlist.json";
                    var json = await System.IO.File.ReadAllTextAsync(filePath);
                    return Results.Content(json, "application/json");
                default:
                    return Results.NotFound("Ваш институт отсутствует");
            }

        }).RequireAuthorization();
        
        materialsGroup.MapPost("/add-material", async (IMaterialsService materialsService,
                [FromForm] AddNewMaterialRequest request,
                HttpRequest httpRequest,
                IMapper mapper,
                IFileService fileService) =>
            {
                var dto = mapper.Map<MaterialDto>(request);
                var materialId = Guid.NewGuid();
                dto.MaterialId = materialId;

                var files = httpRequest.Form.Files;
                if (files.Any())
                {
                    await fileService.AddFiles(files, dto.MaterialId);
                }

                var result = await materialsService.SaveMaterial(dto);

                if (result)
                {
                    return Results.Ok();
                }
                else
                {
                    return Results.BadRequest("Error saving material");
                }

            }).DisableAntiforgery()
            .RequireAuthorization();
        
        return materialsGroup;
    }
}



