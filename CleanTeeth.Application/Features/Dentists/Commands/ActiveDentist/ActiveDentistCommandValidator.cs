using FluentValidation;

namespace CleanTeeth.Application.Features.Dentists.Commands.ActiveDentist;

public class ActiveDentistCommandValidator : AbstractValidator<ActiveDentistCommand>
{
    public ActiveDentistCommandValidator()
    {
        RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("The field {PropertyName} is required.");
    }
}
