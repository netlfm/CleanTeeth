namespace CleanTeeth.Application.Features.DentalOffices.Queries.GetDentalOfficeDetail;

public sealed record GetDentalOfficeDetailResponse
{
    public required Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreationTime { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}