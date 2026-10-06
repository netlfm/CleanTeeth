using CleanTeeth.Domain.Enums;

namespace CleanTeeth.Application.Features.Dentists.Queries.GetDentistDetail;

public sealed record GetDentistDetailResponse
{
    public required Guid Id { get; set; }
    public string? Name { get; set; }
    public Gender? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? LicenseNumber { get; set; }
    public string? Specialty { get; set; }
    public DentistStatus? Status { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreationTime { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
