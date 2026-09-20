using CleanTeeth.Domain.Enums;
using MediatR;

namespace CleanTeeth.Application.Features.Patients.Commands.UpdatePatient;

public class UpdatePatientCommand : IRequest
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required Gender Gender { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
    public required string Address { get; set; }
}
