namespace CleanTeeth.API.Dtos.DentalOffices;

public sealed class UpdateDentalOfficeRequest
{
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
}
