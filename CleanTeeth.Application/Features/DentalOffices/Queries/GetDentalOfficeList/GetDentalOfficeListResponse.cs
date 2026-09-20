namespace CleanTeeth.Application.Features.DentalOffices.Queries.GetDentalOfficeList;

public sealed record GetDentalOfficeListResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreationTime { get; set; }
    public string? LastModifieBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}