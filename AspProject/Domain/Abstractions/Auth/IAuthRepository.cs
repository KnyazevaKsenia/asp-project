using AspProject.Api.Models;
using AspProject.Domain.Models;

namespace AspProject.Domain.Abstractions.Auth;

public interface IAuthRepository
{
    public Task<bool> AddNewUserAsync(RegistrationModel model);
    public Task<bool> IsUserExist(string email);
    public Task<StudentDto>? Authorize(LoginRequest request);
    public Task SaveRefreshToken(Guid userId, string refreshToken);
    public Task<StudentDto?> GetUserByRefreshToken(string accessToken);
}
