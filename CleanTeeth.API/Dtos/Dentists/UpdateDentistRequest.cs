using CleanTeeth.Domain.Enums;

namespace CleanTeeth.API.Dtos.Dentists;

public sealed class UpdateDentistRequest
{
    public required string Name { get; init; }
    public required Gender Gender { get; init; }
    public required string Phone { get; init; }
    public required string Email { get; init; }
    public string? LicenseNumber { get; init; }
    public string? Specialty { get; init; }
}
