using FluentValidation;

namespace CleanTeeth.Application.Features.Treatments.Command.CreateTreatment;

public class CreateTreatmentCommandValidator : AbstractValidator<CreateTreatmentCommand>
{
    public CreateTreatmentCommandValidator()
    {
        RuleFor(x => x.AppointmentId)
        .NotEmpty()
        .WithMessage("The field {PropertyName} is required.");
    }
}
