using MediatR;

namespace CleanTeeth.Application.Features.Treatments.Command.CreateTreatment;

public class CreateTreatmentCommand : IRequest<Guid>
{
    public required Guid AppointmentId { get; set; }
}
