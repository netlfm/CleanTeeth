using MediatR;

namespace CleanTeeth.Application.Features.Dentists.Commands.ActiveDentist;

public class ActiveDentistCommand : IRequest
{
    public required Guid Id { get; set; }
}
