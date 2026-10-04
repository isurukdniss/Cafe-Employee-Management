using FluentValidation;

namespace CafeEmployeeManagement.Application.Features.Auth.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(e => e.Email)
            .NotEmpty().WithMessage("Email is required.");

            RuleFor(e => e.Password)
            .NotEmpty().WithMessage("Password is required.");
        }
    }
}
