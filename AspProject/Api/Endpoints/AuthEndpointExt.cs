using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AspProject.Api.Models;
using AspProject.Configurations;
using AspProject.Domain.Abstractions;
using AspProject.Domain.Abstractions.Auth;
using AspProject.Domain.Models;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AspProject.Api.Endpoints;

public static class AuthEndpointExt
{
    public static IEndpointRouteBuilder MapAuthEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var authGroup = endpoints.MapGroup("auth");
        authGroup.MapPost("register", async (
                        [FromBody] RegistrationModel registModel,
                        HttpContext context,
                        IValidator<RegistrationModel> validator,
                        IAuthService authService) =>
        {
            var validationResult = await validator.ValidateAsync(registModel);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }
            else
            {
                if (await authService.IsUserExist(registModel.Login))
                {
                    return Results.Conflict("Пользователь с такой почтой уже существует");
                }
                if (await authService.AddUser(registModel))
                {
                    return Results.Ok();
                }
                else
                {
                    return Results.Problem("Ошибка при регистрации пользователя. Попробуйте позже.");
                }
            }
        });
        
        authGroup.MapPost("/login", async (IAuthService authService,
            HttpContext context,
            [FromBody] LoginRequest loginRequest,
            AuthConfig config) =>
        {
            if (!await authService.IsUserExist(loginRequest.Login))
            {
                return Results.NotFound("Пользователь не найден.");
            }
            
            var authResult = await authService.AuthorizeUser(loginRequest);
            if (authResult is null)
            {
                return Results.Unauthorized();
            }
            
            var accessToken = GenerateAccessToken(authResult, config);
            
            var refreshToken = GenerateSecureToken();
            
            await authService.SaveRefreshToken(authResult.UserId, refreshToken); 
            
            context.Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });
            return Results.Ok(new { token = accessToken });
        });
        
        authGroup.MapPost("/refresh", async (HttpContext context, IAuthService authService, AuthConfig config) =>
        {
              var userRefreshToken = context.Request.Cookies["RefreshToken"];
              if (!string.IsNullOrEmpty(userRefreshToken))
              {
                  var student = await authService.GetStudentByRefresh(userRefreshToken);
                  if (student is null)
                  {
                      return Results.Unauthorized();
                  }
                  
                  var newAcessToken = GenerateAccessToken(student,config);
                  return Results.Ok(new { Token = newAcessToken });
              }
              
              return Results.Unauthorized();
        });
        
        authGroup.MapPost("/logout", async (HttpContext context, ClaimsPrincipal user, IAuthService authService, AuthConfig config) =>
        {
            foreach (var claim in user.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }
            
            var studentId= user.Claims.FirstOrDefault(c => c.Type == "StudentId")?.Value;
            if (!string.IsNullOrEmpty(studentId))
            {
                var guidId = Guid.Parse(studentId);
                var userId = await authService.GetUserIdByStudentId(guidId);
                await authService.RevokeUserTokens(userId);
            }
            return Results.Ok();
        });
        return authGroup;
        
        string GenerateSecureToken()
        {
            var byteArray = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(byteArray);
            return Convert.ToBase64String(byteArray);
        }
        
        string GenerateAccessToken(StudentDto authResult, AuthConfig config)
        {
            var claims = new List<Claim>
            {
                new("StudentId", authResult.Id.ToString()),
                new("FirstName", authResult.FirstName),
                new("LastName", authResult.LastName),
                new("University", authResult.University),
                new("Institute", authResult.Institute)
            };
            
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.IssuerSignKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var jwt = new JwtSecurityToken(
                issuer: config.Issuer,
                audience: config.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(config.ExpiredAtMinutes)),
                signingCredentials: credentials
            );
            
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            return accessToken;
        }
    }
}