namespace CleanTeeth.API.Dtos.Dentists;

public sealed class CreateDentistRequest
{
    public required string Name { get; init; }
    public required string Gender { get; init; }
    public required string Phone { get; init; }
    public required string Email { get; init; }
    public string? LicenseNumber { get; init; }
    public string? Specialty { get; init; }
}
