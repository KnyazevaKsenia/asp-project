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
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });
            return Results.Ok(new { token = accessToken });
        });
        
        //получаем новый access токен
        authGroup.MapGet("/refresh", async (HttpContext context, IAuthService authService, AuthConfig config) =>
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
                new("UserId", authResult.Id.ToString()),
                new("FirstName", authResult.FirstName),
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