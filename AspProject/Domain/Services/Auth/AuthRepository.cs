using System.Data.Common;
using AspProject.Api.Models;
using AspProject.Domain.Abstractions.Auth;
using AspProject.Domain.Entities;
using AspProject.Domain.Models;
using AspProject.Infrastrastructure.Database;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspProject.Domain.Services;

public class AuthRepository(AppDbContext context, IMapper mapper, IPasswordHasher<User> hasher) : IAuthRepository
{
    public async Task<bool> AddNewUserAsync(RegistrationModel model)
    {
        var user = new User
        {
            Login = model.Login,
            Password = model.Password
        };
        user.Password = hasher.HashPassword(user, model.Password);
        var student = new Student
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            University = model.University,
            Institute = model.Institute,
            User = user 
        };
        try
        {
            await context.Users.AddAsync(user);
            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    //для выдачи токенов при входе
    public async Task<StudentDto>? Authorize(LoginRequest request)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Login == request.Login);
        if (user == null) return null;
        var password = request.Password;
        if (hasher.VerifyHashedPassword(user, user.Password, password) == PasswordVerificationResult.Success)
        {
            var student = await context.Students.FirstOrDefaultAsync(st => st.User.Id == user.Id);
            var result =
                mapper.Map<Student, StudentDto>(student);
            return result;
        }
        
        return null;
    }
    
    public async Task SaveRefreshToken(Guid userId, string refreshToken)
    {
        await context.RefreshTokens.AddAsync(new RefreshToken
        {
            UserId = userId,
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(7)
        });
        await context.SaveChangesAsync();
    }
    
    public async Task<bool> IsUserExist(string login)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Login == login) is not null;
    }
    
    public async Task<StudentDto?> GetUserByRefreshToken(string accessToken)
    {   
        var token = await context.RefreshTokens.FirstOrDefaultAsync(u => u.Token == accessToken && u.IsRevoked == false);
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == token.UserId);
        if (user == null)
        {
            return null;
        }
        var student = await context.Students.FirstOrDefaultAsync(st => st.User.Id == user.Id);
        return student is not null ? mapper.Map<Student, StudentDto>(student) : null;
    }
    
    public async Task RevokeRefreshTokens(Guid userId)
    {
        var tokens = await context.RefreshTokens.Where(u => u.UserId == userId).ToListAsync();
        foreach (var token in tokens)
        {
            token.IsRevoked = true;
        }
        await context.SaveChangesAsync();
    }
        
    public async Task<Guid> GetUserByStudentId(Guid studentId)
    {       
        var student = await context.Students.FirstOrDefaultAsync(st => st.Id == studentId);
        
        if (student== null)
        {
            return Guid.Empty;
        }
        return student.UserId;
    }
    // public async Task<bool> UpdateRefreshToken(Guid id, string newRefreshToken)
    // {
    //     var token = await context.RefreshTokens.FirstOrDefaultAsync(u => u.Id == id);
    //     if (token is null) return false;
    //     await context.RefreshTokens.AddAsync(new RefreshToken
    //     {
    //         UserId = id,
    //         Token = newRefreshToken,
    //         Expires = DateTime.UtcNow.AddDays(7)
    //     });
    //     await context.SaveChangesAsync();
    //     return true;
    // }
}
