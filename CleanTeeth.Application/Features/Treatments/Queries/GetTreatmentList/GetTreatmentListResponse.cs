using CleanTeeth.Domain.Enums;

namespace CleanTeeth.Application.Features.Treatments.Queries.GetTreatmentList;

public class GetTreatmentListResponse
{
    public Guid Id { get; set; }
    public string? PatientName { get; set; }
    public string? DentistName { get; set; }
    public string? DentalOfficeName { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? DurationMinutes { get; set; }
    public TreatmentStatus Status { get; set; }
    public string? Notes { get; set; }
}
