namespace CleanTeeth.Application.Features.DentalOffices.Queries.GetDentalOfficeList;

public sealed record GetDentalOfficeListResponse
{
    public required Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Address { get; init; }
    public Guid? CreatedBy { get; init; }
    public DateTime? CreationTime { get; init; }
    public Guid? LastModifiedBy { get; init; }
    public DateTime? LastModifiedDate { get; init; }
}