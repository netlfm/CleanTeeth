using MediatR;

namespace CleanTeeth.Application.Features.Patients.Commands.DeletePatient;

public class DeletePatientCommand : IRequest
{
    public Guid Id { get; set; }
}
