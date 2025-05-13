using AspProject.Api.Models;
using FluentValidation;
namespace AspProject.Configurations.Validator;

public class LogInValidator : AbstractValidator<LoginRequest>
{
    public LogInValidator()
    {
        RuleFor(log => log.Login)
            .NotEmpty()
            .WithMessage("Login field is required")
            .EmailAddress();
        
        RuleFor(log => log.Password)
            .NotEmpty()
            .WithMessage("Password field is required")
            .MinimumLength(8).WithMessage("Пароль должен быть не менее 8 символов")
            .Matches("[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву")
            .Matches("[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву")
            .Matches("[0-9]").WithMessage("Пароль должен содержать хотя бы одну цифру")
            .Matches("[^a-zA-Z0-9]").WithMessage("Пароль должен содержать хотя бы один специальный символ");
    }
}