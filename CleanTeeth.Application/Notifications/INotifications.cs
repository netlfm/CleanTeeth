namespace CleanTeeth.Application.Notifications;

public interface INotifications
{
    Task SendAppointmentReminder(AppointmentReminderDTO appointmentReminderDTO);
    Task SendTreatmentReport(TreatmentReportDTO treatmentReportDTO);
}
