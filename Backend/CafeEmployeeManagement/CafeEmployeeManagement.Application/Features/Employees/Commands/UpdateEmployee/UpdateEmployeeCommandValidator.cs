using CafeEmployeeManagement.Domain.Enums;
using FluentValidation;

namespace CafeEmployeeManagement.Application.Features.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            RuleFor(e => e.Name)
            .NotEmpty().WithMessage("Name is required.");

            RuleFor(e => e.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("Email address format is not valid.");

            RuleFor(e => e.PhoneNumber.ToString())
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^[89]\d{7}$")
                .WithMessage("Phone number must be 8 digits and start with either 9 or 8.");

            RuleFor(e => e.Gender)
                .NotEmpty().WithMessage("Gender is required")
                .Must(g => g == Gender.Male || g == Gender.Female)
                .WithMessage($"Gender must be {Gender.Male} or {Gender.Female}");

            RuleFor(x => x.CafeId)
               .NotEmpty().WithMessage("CafeId is required.");

        }
    }
}
