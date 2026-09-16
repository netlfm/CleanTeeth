namespace CleanTeeth.Application.Features.Dentists.Queries.GetDentistDetail;

public sealed record GetDentistDetailResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Gender { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
    public string? LicenseNumber { get; set; }
    public string? Specialty { get; set; }
    public string? Status { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreationTime { get; set; }
    public string? LastModifieBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
