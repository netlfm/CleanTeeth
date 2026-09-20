using CleanTeeth.Domain.Enums;

namespace CleanTeeth.Application.Features.Patients.Queries.GetPatientList;

public sealed record GetPatientListReponse
{
    public Guid Id { get; init; }
    public string? PatientNumber { get; init; }
    public string? Name { get; init; }
    public DateOnly? DateOfBirth { get; init; }
    public Gender? Gender { get; init; }
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public string? Address { get; init; }
    public string? CreatedBy { get; init; }
    public DentistStatus? Status { get; init; }
    public DateTime? CreationTime { get; init; }
    public string? LastModifieBy { get; init; }
    public DateTime? LastModifiedDate { get; init; }
}
