using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;
using MediatR;

namespace CleanTeeth.Application.Features.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
{
    private readonly IAppointmentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAppointmentCommandHandler(IAppointmentRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
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
        try
        {
            var result = await _repository.Add(appointment, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            return result.Id;
        }
        catch (Exception)
        {
            await _unitOfWork.Rollback(cancellationToken);
            throw;
        }
    }
}
