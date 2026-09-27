using MediatR;

namespace CleanTeeth.Application.Features.Treatments.Command.CompleteTreatment;

public class CompleteTreatmentCommand : IRequest
{
    public required Guid Id { get; set; }
    public required string Notes { get; set; }
}
