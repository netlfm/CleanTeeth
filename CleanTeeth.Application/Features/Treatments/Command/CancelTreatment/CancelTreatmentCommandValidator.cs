using FluentValidation;

namespace CleanTeeth.Application.Features.Treatments.Command.CancelTreatment;

public class CancelTreatmentCommandValidator : AbstractValidator<CancelTreatmentCommand>
{
    public CancelTreatmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("The field {PropertyName} is required.");
    }
}
