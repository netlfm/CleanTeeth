namespace CleanTeeth.API.Dtos.Treatments;

public sealed class CreateTreatmentRequest
{
    public required Guid AppointmentId { get; init; }
}
