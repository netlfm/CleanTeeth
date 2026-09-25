namespace CleanTeeth.API.Dtos.Appointment;

public sealed class CreateAppointmentRequest
{
    public required Guid PatientId { get; init; }
    public required Guid DentistId { get; init; }
    public required Guid DentalOfficeId { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
}
