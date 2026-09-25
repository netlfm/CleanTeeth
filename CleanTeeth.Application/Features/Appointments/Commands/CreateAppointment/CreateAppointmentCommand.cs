using MediatR;

namespace CleanTeeth.Application.Features.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommand : IRequest<Guid>
{
    public required Guid PatientId { get; set; }
    public required Guid DentistId { get; set; }
    public required Guid DentalOfficeId { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
}
