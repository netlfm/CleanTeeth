using MediatR;

namespace CleanTeeth.Application.Features.Appointments.Commands.CancelAppointment;

public class CancelAppointmentCommand : IRequest
{
    public required Guid Id { get; set; }
}
