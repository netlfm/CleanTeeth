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
        string message = $"[Appointment Reminder] This is a reminder that your appointment is coming up. Time: {appointmentReminderDTO.Date:yyyy-MM-dd HH:mm}.";
        await SendPhoneMessage(appointmentReminderDTO.PatientPhone, message);
    }

    private async Task SendPhoneMessage(string phone, string message)
    {
        await Task.Delay(800);
        _logger.LogInformation("Message sent successfully。PhoneNumber: {Phone}, Message: {Message}", phone, message);
    }
}
