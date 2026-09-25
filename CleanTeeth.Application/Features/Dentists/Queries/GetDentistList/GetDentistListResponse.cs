using CleanTeeth.Domain.Enums;

namespace CleanTeeth.Application.Features.Dentists.Queries.GetDentistList;

public sealed record GetDentistListResponse
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public Gender? Gender { get; init; }
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public string? LicenseNumber { get; init; }
    public string? Specialty { get; init; }
    public DentistStatus? Status { get; init; }
    public string? CreatedBy { get; init; }
    public DateTime? CreationTime { get; init; }
    public string? LastModifieBy { get; init; }
    public DateTime? LastModifiedDate { get; init; }
}
