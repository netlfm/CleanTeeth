using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Application.Notifications;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;
using MediatR;

namespace CleanTeeth.Application.Features.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
{
    private readonly IAppointmentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotifications _notifications;
    public CreateAppointmentCommandHandler(IAppointmentRepository repository, IUnitOfWork unitOfWork, INotifications notification)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _notifications = notification;
    }
    public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var conflict = await _repository.FindConflict(
            request.PatientId, request.DentistId, request.DentalOfficeId,
            request.StartDate, request.EndDate, cancellationToken);
        if (conflict is not null)
        {
            string resource = string.Empty;
            if (conflict.DentistId == request.DentistId)
            {
                resource = "dentist";
            }
            else if (conflict.PatientId == request.PatientId)
            {
                resource = "patient";
            }
            else
            {
                resource = "dental office";
            }
            throw new AppointmentOverlapException($"The {resource} is already booked during the requested time.");
        }
        var timeInterval = new TimeInterval(request.StartDate, request.EndDate);
        var appointment = new Appointment(request.PatientId, request.DentistId, request.DentalOfficeId, timeInterval);
        Guid? id = null;
        try
        {
            var result = await _repository.Add(appointment, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            id = result.Id;
        }
        catch (Exception)
        {
            await _unitOfWork.Rollback(cancellationToken);
            throw;
        }
        var appointmentdto = await _repository.GetById(id.Value, cancellationToken);
        var dto = new AppointmentReminderDTO
        {
            Id = id.Value,
            DentalOffice = appointmentdto!.DentalOffice!.Name,
            Dentist = appointmentdto.Dentist!.Name,
            Patient = appointmentdto.Patient!.Name,
            PatientPhone = appointmentdto.Patient.Phone.Value,
            Date = appointmentdto.TimeInterval.Start
        };
        await _notifications.SendAppointmentReminder(dto);
        return id.Value;
    }
}
