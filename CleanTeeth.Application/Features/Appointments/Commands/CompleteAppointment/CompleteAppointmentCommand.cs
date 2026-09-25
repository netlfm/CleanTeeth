using MediatR;

namespace CleanTeeth.Application.Features.Appointments.Commands.CompleteAppointment;

public class CompleteAppointmentCommand : IRequest
{
    public required Guid Id { get; set; }
}
