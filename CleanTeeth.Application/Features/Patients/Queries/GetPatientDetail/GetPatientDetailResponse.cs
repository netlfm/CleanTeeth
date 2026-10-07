using CleanTeeth.Domain.Enums;

namespace CleanTeeth.Application.Features.Patients.Queries.GetPatientDetail;

public class GetPatientDetailResponse
{
    public required Guid Id { get; set; }
    public string? PatientNumber { get; set; }
    public string? Name { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreationTime { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}