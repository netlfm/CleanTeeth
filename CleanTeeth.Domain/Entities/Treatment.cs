using CleanTeeth.Domain.Common;
using CleanTeeth.Domain.Enums;
using CleanTeeth.Domain.Exceptions;

namespace CleanTeeth.Domain.Entities;

public class Treatment : AuditableEntity
{
    public Guid Id { get; private set; }
    public Guid AppointmentId { get; private set; }
    public DateTime? StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public int DurationMinutes { get; private set; }
    public TreatmentStatus Status { get; private set; }
    public string? Notes { get; private set; }
    public Appointment? Appointment { get; private set; }
    private Treatment()
    {

    }
    public Treatment(Guid appointmentId)
    {
        Id = Guid.CreateVersion7();
        AppointmentId = appointmentId;
        StartTime = DateTime.UtcNow;
        Status = TreatmentStatus.InProgress;
    }

    public void CompleteTreatment(DateTime endTime, string? finalNotes)
    {
        if (Status != TreatmentStatus.InProgress)
        {
            throw new BusinessRuleException("Only an in-progress treatment can be completed.");
        }
        DurationMinutes = (int)(endTime - StartTime!.Value).TotalMinutes;
        Notes = finalNotes;
        Status = TreatmentStatus.Completed;
        LastModifiedDate = DateTime.UtcNow;
    }

    public void CancelTreatment()
    {
        if (Status == TreatmentStatus.Completed)
        {
            throw new BusinessRuleException("Completed treatment cannot be cancelled.");
        }
        if (Status == TreatmentStatus.Cancelled)
        {
            throw new BusinessRuleException("Treatment has already been cancelled.");
        }
        Status = TreatmentStatus.Cancelled;
        LastModifiedDate = DateTime.UtcNow;
    }
}