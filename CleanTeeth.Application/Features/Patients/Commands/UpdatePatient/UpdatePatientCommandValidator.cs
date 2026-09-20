using FluentValidation;

namespace CleanTeeth.Application.Features.Patients.Commands.UpdatePatient;

public class UpdatePatientCommandValidator:AbstractValidator<UpdatePatientCommand>
{
    public UpdatePatientCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");
        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("DateOfBirth is required.");
        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Gender is invalid.");
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("The field {PropertyName} is required.")
            .MaximumLength(30).WithMessage("The field {PropertyName} must not exceed 30 characters.")
            .Matches(@"^\+?[\d\s\-/()]+$")
            .WithMessage("The {PropertyName} contains invalid characters.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.");
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(500).WithMessage("Address must not exceed 500 characters.");
    }
}
