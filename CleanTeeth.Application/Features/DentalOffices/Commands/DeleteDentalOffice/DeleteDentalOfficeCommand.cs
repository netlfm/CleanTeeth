using MediatR;

namespace CleanTeeth.Application.Features.DentalOffices.Commands.DeleteDentalOffice;

public class DeleteDentalOfficeCommand : IRequest
{
    public Guid Id { get; set; }
}
