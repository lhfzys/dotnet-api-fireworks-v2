using FluentValidation;

namespace Fireworks.Application.Features.system.Auth.Login;

public class LoginValidator:AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleLevelCascadeMode = ClassLevelCascadeMode;
        RuleFor(x => x.UserName).NotEmpty().NotNull().MaximumLength(50);
        RuleFor(x => x.Password).NotEmpty().NotNull().MinimumLength(6);
    }
}