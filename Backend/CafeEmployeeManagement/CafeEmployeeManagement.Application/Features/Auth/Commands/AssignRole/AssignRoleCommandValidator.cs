using CafeEmployeeManagement.Domain.Constants;
using FluentValidation;

namespace CafeEmployeeManagement.Application.Features.Auth.Commands.AssignRole
{
    public class AssignRoleCommandValidator : AbstractValidator<AssignRoleCommand>
    {
        public AssignRoleCommandValidator()
        {
            RuleFor(e => e.Email)
            .NotEmpty().WithMessage("Email is required.");

            RuleFor(e => e.Role)
            .NotEmpty().WithMessage("Role is required.")
            .Must(role => Roles.All.Contains(role))
            .WithMessage($"Role must be one of: {string.Join(", ", Roles.All)}.");
        }
    }
}
