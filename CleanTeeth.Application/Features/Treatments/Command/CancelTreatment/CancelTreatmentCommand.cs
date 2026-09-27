using MediatR;

namespace CleanTeeth.Application.Features.Treatments.Command.CancelTreatment;

public class CancelTreatmentCommand : IRequest
{
    public required Guid Id { get; set; }
}
