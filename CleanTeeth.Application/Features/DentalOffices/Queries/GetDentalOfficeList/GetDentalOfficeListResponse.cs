namespace CleanTeeth.Application.Features.DentalOffices.Queries.GetDentalOfficeList;

public sealed record GetDentalOfficeListResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreationTime { get; set; }
    public string? LastModifieBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}