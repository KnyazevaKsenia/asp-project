using System.Security.Claims;
using AspProject.Api.Models;
using AspProject.Domain.Abstractions;
using AspProject.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace AspProject.Api.Endpoints;

public static class MyPageEndpointExt
{
    public static IEndpointRouteBuilder MapMyPageEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var profileGroup = endpoints.MapGroup("profile");
        profileGroup.MapGet("materials", async (ClaimsPrincipal student, IMaterialsService materilService, IMapper mapper) =>
        {
            var studentId= student.Claims.FirstOrDefault(c => c.Type == "StudentId")?.Value;
            if (!string.IsNullOrEmpty(studentId))
            {
                var materialsDto = await materilService.GetMaterialsByStudentId(Guid.Parse(studentId));
                List<MaterialResponse> result = mapper.Map<List<MaterialResponseDto>, List<MaterialResponse>>(materialsDto) ;
                return Results.Json(new { materials = result });
            }
            return Results.Problem();
        }).RequireAuthorization();
        
        profileGroup.MapDelete("delete/{materialId}", async (Guid materialId, 
                                            IMaterialsService materialsService) =>
        {
            if (await materialsService.DeleteMaterialById(materialId))
            {
                return Results.Ok();
            }

            return Results.NotFound();
        }).RequireAuthorization();
        
        profileGroup.MapPost("change/{materialId}", async (Guid materialId,
                                    [FromForm] AddNewMaterialRequest request, 
                                    IMaterialsService materialsService,
                                    HttpRequest httpRequest,
                                    IFileService fileService) =>
        {
            Console.WriteLine(materialId);
            var files = httpRequest.Form.Files;
            if (await materialsService.ChangeMaterial(request, materialId))
            {
                if (files.Any())
                {
                    await fileService.AddFiles(files, materialId);
                }
                return Results.Ok();
            }
            return Results.NotFound();
        }).RequireAuthorization().DisableAntiforgery();
        return profileGroup;
    }
}


