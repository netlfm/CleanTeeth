using FluentValidation;

namespace CleanTeeth.Application.Features.Dentists.Commands.InactiveDentist;

public class InactiveDentistCommandValidator : AbstractValidator<InactiveDentistCommand>
{
    public InactiveDentistCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("The field {PropertyName} is required.");
    }
}
