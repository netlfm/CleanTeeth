using MediatR;

namespace CleanTeeth.Application.Features.Dentists.Commands.InactiveDentist;

public class InactiveDentistCommand : IRequest
{
    public required Guid Id { get; set; }
}
