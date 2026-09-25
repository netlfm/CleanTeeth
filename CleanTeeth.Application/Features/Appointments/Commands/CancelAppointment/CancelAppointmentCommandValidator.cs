using FluentValidation;

namespace CleanTeeth.Application.Features.Appointments.Commands.CancelAppointment;

internal class CancelAppointmentCommandValidator : AbstractValidator<CancelAppointmentCommand>
{
    public CancelAppointmentCommandValidator()
    {
        RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("The field {PropertyName} is required.");
    }
}
