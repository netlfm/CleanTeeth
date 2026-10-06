namespace CleanTeeth.Application.Features.Appointments.Queries.GetAppointmentList;

public sealed class GetAppointmentListResponse
{
    public required Guid Id { get; set; }
    public required string PatientName { get; set; }
    public required string DentistName { get; set; }
    public required string DentalOfficeName { get; set; }
    public required string Status { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreationTime { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
