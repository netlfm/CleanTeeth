namespace CleanTeeth.API.Dtos.DentalOffices;

public sealed class UpdateDentalOfficeRequest
{
    public required string Name { get; init; }
    public required string Address { get; init; }
    public required string Phone { get; init; }
    public required string Email { get; init; }
}
