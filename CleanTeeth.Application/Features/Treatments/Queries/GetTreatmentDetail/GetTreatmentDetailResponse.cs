using CleanTeeth.Domain.Enums;

namespace CleanTeeth.Application.Features.Treatments.Queries.GetTreatmentDetail;

public class GetTreatmentDetailResponse
{
    public required Guid Id { get; set; }
    public string PatientName { get; set; } = null!;
    public string DentistName { get; set; } = null!;
    public string DentalOfficeName { get; set; } = null!;
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int DurationMinutes { get; set; }
    public TreatmentStatus Status { get; set; }
    public string? Notes { get; set; }

    public string? CreatedBy { get; set; }
    public DateTime? CreationTime { get; set; }
    public string? LastModifieBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
