using FluentValidation;

namespace CleanTeeth.Application.Features.Appointments.Commands.CompleteAppointment;

public class CompleteAppointmentCommandValidator:AbstractValidator<CompleteAppointmentCommand>
{
    public CompleteAppointmentCommandValidator()
    {
        RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("The field {PropertyName} is required.");
    }
}
