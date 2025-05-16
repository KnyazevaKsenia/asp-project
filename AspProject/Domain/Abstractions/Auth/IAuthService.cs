using AspProject.Api.Models;
using AspProject.Domain.Models;

namespace AspProject.Domain.Abstractions.Auth;

public interface IAuthService
{
    public Task<bool> AddUser(RegistrationModel registrationModel);
    public Task<bool> IsUserExist(string email);
    public Task<StudentDto?> AuthorizeUser(LoginRequest loginRequest);
    public Task SaveRefreshToken(Guid userId, string refreshToken);
    public Task<StudentDto?> GetStudentByRefresh(string refreshToken);
    public Task RevokeUserTokens(Guid userId);
    public Task<Guid> GetUserIdByStudentId(Guid studentId);
}