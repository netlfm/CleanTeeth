using CleanTeeth.Domain.Enums;

namespace CleanTeeth.API.Dtos.Patients;

public class UpdatePatientRequest
{
    public required string Name { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public required Gender Gender { get; init; }
    public required string Phone { get; init; }
    public required string Email { get; init; }
    public required string Address { get; init; }
}
