using FluentValidation;

namespace CafeEmployeeManagement.Application.Features.Cafes.Commands.UpdateCafe
{
    public class UpdateCafeCommandValidator : AbstractValidator<UpdateCafeCommand>
    {
        public UpdateCafeCommandValidator()
        {
            RuleFor(e => e.Name)
            .NotEmpty().WithMessage("Name is required.");

            RuleFor(e => e.Description)
            .NotEmpty().WithMessage("Description is required.");

            RuleFor(e => e.Location)
            .NotEmpty().WithMessage("Location is required.");

            RuleFor(e => e.Id)
           .NotEmpty().WithMessage("Id is required.");
        }
    }
}
