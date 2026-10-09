using CleanTeeth.Application.Notifications;
using Microsoft.Extensions.Logging;

namespace CleanTeeth.Infrastructure.Notifications;

public class MessageService : INotifications
{
    private readonly ILogger<INotifications> _logger;
    public MessageService(ILogger<INotifications> logger)
    {
        _logger = logger;
    }
    public async Task SendAppointmentReminder(AppointmentReminderDTO appointmentReminderDTO)
    {
        if (appointmentReminderDTO == null)
        {
            throw new ArgumentNullException(nameof(appointmentReminderDTO));
        }
        string message = $"[Appointment Confirmation] Dear {appointmentReminderDTO.Patient}, your appointment with Dr. {appointmentReminderDTO.Dentist} at {appointmentReminderDTO.DentalOffice} is confirmed for {appointmentReminderDTO.Date:yyyy-MM-dd HH:mm}.";
        await SendPhoneMessage(appointmentReminderDTO.PatientPhone, message);
    }

    public async Task SendTreatmentReport(TreatmentReportDTO treatmentReportDTO)
    {
        if (treatmentReportDTO == null)
        {
            throw new ArgumentNullException(nameof(treatmentReportDTO));
        }
        string message = $"[Treatment Report] Dear {treatmentReportDTO.Patient}, your treatment at {treatmentReportDTO.DentalOffice} by Dr. {treatmentReportDTO.Dentist} has been completed on {treatmentReportDTO.CompletedAt:yyyy-MM-dd HH:mm}. Duration: {treatmentReportDTO.DurationMinutes} min.";
        if (!string.IsNullOrWhiteSpace(treatmentReportDTO.Notes))
        {
            message += $" Diagnosis: {treatmentReportDTO.Notes}";
        }
        await SendPhoneMessage(treatmentReportDTO.PatientPhone, message);
    }

    private async Task SendPhoneMessage(string phone, string message)
    {
        await Task.Delay(800);
        _logger.LogInformation("Message sent successfully。PhoneNumber: {Phone}, Message: {Message}", phone, message);
    }
}
