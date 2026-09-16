using FluentValidation;

namespace CleanTeeth.Application.Features.Dentists.Commands.UpdateDentist;

public sealed class UpdateDentistCommandValidator : AbstractValidator<UpdateDentistCommand>
{
    public UpdateDentistCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("The field {PropertyName} is required.")
            .MaximumLength(150).WithMessage("The field {PropertyName} must not exceed 150 characters.");
        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("The field {PropertyName} is required.")
            .MaximumLength(50).WithMessage("The field {PropertyName} must not exceed 50 characters.")
            .EmailAddress().WithMessage("The {PropertyName} format is invalid.");
        RuleFor(p => p.Phone)
            .NotEmpty().WithMessage("The field {PropertyName} is required.")
            .MaximumLength(30).WithMessage("The field {PropertyName} must not exceed 30 characters.")
            .Matches(@"^\+?[\d\s\-/()]+$").WithMessage("The {PropertyName} contains invalid characters.");
        RuleFor(p => p.Gender)
            .IsInEnum().WithMessage("The field {PropertyName} has an invalid value.");
        RuleFor(p => p.LicenseNumber)
            .MaximumLength(50).WithMessage("The field {PropertyName} must not exceed 50 characters.")
            .Matches(@"^[a-zA-Z0-9\-]+$").When(p => !string.IsNullOrEmpty(p.LicenseNumber))
            .WithMessage("The {PropertyName} must contain only alphanumeric characters and hyphens.");
        RuleFor(p => p.Specialty)
            .MaximumLength(100).WithMessage("The field {PropertyName} must not exceed 100 characters.");
    }
}
