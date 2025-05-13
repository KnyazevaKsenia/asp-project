using AspProject.Api.Models;
using AspProject.Domain.Abstractions.Auth;
using AspProject.Domain.Models;
using AspProject.Infrastrastructure.Database;

namespace AspProject.Domain.Services;

public class AuthService(IAuthRepository authRepository) : IAuthService
{
    public async Task<bool> AddUser(RegistrationModel registrationModel)
    {
        return await authRepository.AddNewUserAsync(registrationModel);
    }

    public async Task<bool> IsUserExist(string email)
    {
        return await authRepository.IsUserExist(email);
    }
    
    public async Task<StudentDto?> AuthorizeUser(LoginRequest loginRequest)
    {
        return await authRepository.Authorize(loginRequest);
    }

    public async Task SaveRefreshToken(Guid userId, string refreshToken)
    {
        await authRepository.SaveRefreshToken(userId, refreshToken);
    }

    public async Task<StudentDto?> GetStudentByRefresh(string refreshToken)
    {
        return await authRepository.GetUserByRefreshToken(refreshToken);
    }
}