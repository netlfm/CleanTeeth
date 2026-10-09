namespace CleanTeeth.Application.Notifications;

public class TreatmentReportDTO : AppointmentMessageDataDTO
{
    public required int DurationMinutes { get; set; }
    public string? Notes { get; set; }
    public required DateTime CompletedAt { get; set; }
}
