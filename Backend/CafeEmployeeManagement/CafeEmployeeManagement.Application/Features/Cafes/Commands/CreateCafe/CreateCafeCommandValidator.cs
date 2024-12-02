using FluentValidation;

namespace CafeEmployeeManagement.Application.Features.Cafes.Commands.CreateCafe
{
    public class CreateCafeCommandValidator : AbstractValidator<CreateCafeCommand>
    {
        public CreateCafeCommandValidator()
        {
            RuleFor(e => e.Name)
            .NotEmpty().WithMessage("Name is required.");

            RuleFor(e => e.Description)
            .NotEmpty().WithMessage("Description is required.");

            RuleFor(e => e.Location)
            .NotEmpty().WithMessage("Location is required.");

        }
    }
}
