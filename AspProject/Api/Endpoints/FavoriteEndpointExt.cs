using System.Security.Claims;
using AspProject.Domain.Abstractions;

namespace AspProject.Api.Endpoints;

public static class FavoriteEndpointExt
{
    public static IEndpointRouteBuilder MapFavoriteEndpoints(this IEndpointRouteBuilder endpoints)
    {   
        var favoriteGroup = endpoints.MapGroup("favorites");
        favoriteGroup.MapPost("add/{materialId}", async (ClaimsPrincipal student,Guid materialId, IFavoriteService favoriteService) =>
        {
            var studentId = student.Claims.FirstOrDefault(c => c.Type == "StudentId")?.Value;
            if (studentId != null)
            {
                var id = Guid.Parse(studentId);
                var result = await favoriteService.AddFavorite(materialId, id);
                if (result)
                {
                    return Results.Ok();
                }
            }
            return Results.BadRequest();
        }).RequireAuthorization();
        favoriteGroup.MapGet("get-student-info", async (ClaimsPrincipal student, IFavoriteService favoriteService) =>
        {
            var studentId = student.Claims.FirstOrDefault(c => c.Type == "StudentId")?.Value;
            if (Guid.TryParse(studentId, out var id))
            {
                var result = await favoriteService.GetStudentFavoritesIds(id);
                return Results.Json(result);
            }
            return Results.BadRequest();
            
        }).RequireAuthorization();
        favoriteGroup.MapGet("remove/{materialId}", async (ClaimsPrincipal student,Guid materialId, IFavoriteService favoriteService) =>
        {
            var studentId = student.Claims.FirstOrDefault(c => c.Type == "StudentId")?.Value;
            if (Guid.TryParse(studentId, out var id))
            {
                if (await favoriteService.RemoveFavorite(id,materialId))
                {
                    return Results.Ok();
                }
                return Results.NotFound();
            }
           
            return Results.BadRequest();
        }).RequireAuthorization();
        
        favoriteGroup.MapGet("", async (ClaimsPrincipal student, IFavoriteService favoriteService, IFileService fileService) =>
        {
            if (Guid.TryParse(student.Claims.FirstOrDefault(c => c.Type == "StudentId")?.Value, out Guid studentId))
            {
                var result = await favoriteService.GetStudentFavorites(studentId);
                return Results.Json(result);
            }
            return Results.BadRequest();
            
        }).RequireAuthorization();
        return favoriteGroup;
    }
}