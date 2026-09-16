using CleanTeeth.Domain.Enums;
using MediatR;

namespace CleanTeeth.Application.Features.Patients.Commands.CreatePatient;

public class CreatePatientCommand : IRequest<Guid>
{
    public required string PatientNumber { get; set; }
    public required string Name { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public required Gender Gender { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
    public required string Address { get; set; }
}
