using FluentValidation;

namespace CleanTeeth.Application.Features.DentalOffices.Commands.CreateDentalOffice;

public sealed class CreateDentalOfficeCommandValidator : AbstractValidator<CreateDentalOfficeCommand>
{
    public CreateDentalOfficeCommandValidator()
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
        RuleFor(p => p.Address)
            .NotEmpty().WithMessage("The field {PropertyName} is required.")
            .MaximumLength(200).WithMessage("The field {PropertyName} must not exceed 200 characters.");
    }
}
