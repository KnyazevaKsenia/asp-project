using AspProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
namespace AspProject.Domain.Services;

using Microsoft.AspNetCore.Identity;

public class PasswordHasher : IPasswordHasher<User>
{
    private readonly PasswordHasher<User> _hasher = new();

    public string HashPassword(User user, string password)
    {
        return _hasher.HashPassword(user, password);
    }
    
    public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
    {
        return _hasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
    }
}

