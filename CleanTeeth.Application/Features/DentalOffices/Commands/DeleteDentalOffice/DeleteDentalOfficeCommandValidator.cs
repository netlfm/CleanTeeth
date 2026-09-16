using FluentValidation;

namespace CleanTeeth.Application.Features.DentalOffices.Commands.DeleteDentalOffice;

public class DeleteDentalOfficeCommandValidator : AbstractValidator<DeleteDentalOfficeCommand>
{
    public DeleteDentalOfficeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("The field {PropertyName} is required.");
    }
}
