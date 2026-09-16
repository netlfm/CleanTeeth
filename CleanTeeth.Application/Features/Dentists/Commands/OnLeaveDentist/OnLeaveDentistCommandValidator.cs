using FluentValidation;

namespace CleanTeeth.Application.Features.Dentists.Commands.OnLeaveDentist;

public class OnLeaveDentistCommandValidator : AbstractValidator<OnLeaveDentistCommand>
{
    public OnLeaveDentistCommandValidator()
    {
        RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("The field {PropertyName} is required.");
    }
}
