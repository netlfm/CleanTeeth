using MediatR;

namespace CleanTeeth.Application.Features.Dentists.Commands.DeleteDentist;

public class DeleteDentistCommand : IRequest
{
    public Guid Id { get; set; }
}
