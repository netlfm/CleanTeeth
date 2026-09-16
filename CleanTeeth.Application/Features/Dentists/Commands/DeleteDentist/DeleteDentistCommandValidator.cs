using FluentValidation;

namespace CleanTeeth.Application.Features.Dentists.Commands.DeleteDentist;

public class DeleteDentistCommandValidator : AbstractValidator<DeleteDentistCommand>
{
    public DeleteDentistCommandValidator()
    {
        RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("The field {PropertyName} is required.");
    }
}
