using MediatR;

namespace CleanTeeth.Application.Features.Dentists.Commands.OnLeaveDentist;

public class OnLeaveDentistCommand : IRequest
{
    public required Guid Id { get; set; }
}
