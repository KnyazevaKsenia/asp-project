using AspProject.Api.Models;
using FluentValidation;

namespace AspProject.Configurations.Validator;

public class RegistrationValidator : AbstractValidator<RegistrationModel>
{
    public RegistrationValidator()
    {
        RuleFor(reg => reg.Login)
            .NotEmpty().WithMessage("Электронная почта обязательна")
            .Length(3, 50)
            .EmailAddress()
            .WithMessage("Некорректный адрес электронной почты");
        
        RuleFor(reg => reg.Password)
            .NotEmpty().WithMessage("Пароль обязателен")
            .MinimumLength(8).WithMessage("Пароль должен быть не менее 8 символов")
            .Matches("[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву")
            .Matches("[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву")
            .Matches("[0-9]").WithMessage("Пароль должен содержать хотя бы одну цифру")
            .Matches("[^a-zA-Z0-9]").WithMessage("Пароль должен содержать хотя бы один специальный символ");
        
        RuleFor(reg=> reg.FirstName)
            .NotEmpty()
            .WithMessage("Имя обязательно")
            .Length(2, 50)
            .Matches("^[А-ЯЁA-Z][а-яёa-z-]+$").WithMessage("Имя должно начинаться с заглавной буквы и содержать только буквы и дефис");
        
        RuleFor(reg=> reg.LastName)
            .Length(2, 50)
            .Matches("^[А-ЯЁA-Z][а-яёa-z-]+$").WithMessage("Фамилия должна начинаться с заглавной буквы и содержать только буквы и дефис");

    }
}