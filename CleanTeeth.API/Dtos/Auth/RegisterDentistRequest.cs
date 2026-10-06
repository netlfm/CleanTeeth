using CleanTeeth.Domain.Enums;

namespace CleanTeeth.API.Dtos.Auth;

public class RegisterDentistRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string Name { get; init; }
    public required Gender Gender { get; init; }
    public required string Phone { get; init; }
    public string? LicenseNumber { get; init; }
    public string? Specialty { get; init; }
}
