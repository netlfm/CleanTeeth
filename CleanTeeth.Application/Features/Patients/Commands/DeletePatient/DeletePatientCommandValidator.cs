using FluentValidation;

namespace CleanTeeth.Application.Features.Patients.Commands.DeletePatient;

internal class DeletePatientCommandValidator : AbstractValidator<DeletePatientCommand>
{
    public DeletePatientCommandValidator()
    {
        RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("The field {PropertyName} is required.");
    }
}
