using CleanTeeth.Domain.Common;
using CleanTeeth.Domain.Enums;
using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.Domain.Entities;

public class Appointment : AuditableEntity
{
    public Guid Id { get; private set; }
    public Guid PatientId { get; private set; }
    public Guid DentistId { get; private set; }
    public Guid DentalOfficeId { get; private set; }
    public AppointmentStatus Status { get; private set; }
    public TimeInterval TimeInterval { get; private set; } = null!;
    public Patient? Patient { get; private set; }
    public Dentist? Dentist { get; private set; }
    public DentalOffice? DentalOffice { get; private set; }
    private Appointment()
    {

    }
    public Appointment(Guid patientId, Guid dentistId, Guid dentalofficeId, TimeInterval timeInterval)
    {
        if (timeInterval.Start < DateTime.UtcNow)
        {
            throw new BusinessRuleException($"The {nameof(timeInterval.Start)} must be less than or equal to the current date and time.");
        }
        PatientId = patientId;
        DentistId = dentistId;
        DentalOfficeId = dentalofficeId;
        TimeInterval = timeInterval;
        Status = AppointmentStatus.Scheduled;
        Id = Guid.CreateVersion7();
    }

    public void Cancel()
    {
        if (Status != AppointmentStatus.Scheduled)
        {
            throw new BusinessRuleException($"The appointment cannot be canceled because it is not in the {AppointmentStatus.Scheduled} status.");
        }
        Status = AppointmentStatus.Cancelled;
    }
    public void Complete()
    {
        if (Status != AppointmentStatus.Scheduled)
        {
            throw new BusinessRuleException($"The appointment cannot be completed because it is not in the {AppointmentStatus.Scheduled} status.");
        }
        Status = AppointmentStatus.Completed;
    }
}
