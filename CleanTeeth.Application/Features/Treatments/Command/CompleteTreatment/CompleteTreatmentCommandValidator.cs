using FluentValidation;

namespace CleanTeeth.Application.Features.Treatments.Command.CompleteTreatment;

public class CompleteTreatmentCommandValidator : AbstractValidator<CompleteTreatmentCommand>
{
    public CompleteTreatmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("The field {PropertyName} is required.");
        RuleFor(x => x.Notes)
            .NotEmpty()
            .WithMessage("The field {PropertyName} is required.")
            .MaximumLength(1000)
            .WithMessage("The field {PropertyName} must not exceed 1000 characters.");
    }
}
