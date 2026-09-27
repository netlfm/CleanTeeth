using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Enums;
using CleanTeeth.Domain.Exceptions;
using MediatR;

namespace CleanTeeth.Application.Features.Treatments.Command.CreateTreatment;

public class CreateTreatmentCommandHandler : IRequestHandler<CreateTreatmentCommand, Guid>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ITreatmentRepository _treatmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateTreatmentCommandHandler(ITreatmentRepository treatmentRepository, IAppointmentRepository appointmentRepository, IUnitOfWork unitOfWork)
    {
        _appointmentRepository = appointmentRepository;
        _treatmentRepository = treatmentRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Guid> Handle(CreateTreatmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetById(request.AppointmentId, cancellationToken);
        if (appointment is null)
        {
            throw new BusinessRuleException($"Appointment does not exist.");
        }
        var exists = await _treatmentRepository.ExistsByAppointmentId(request.AppointmentId, cancellationToken);
        if (exists)
        {
            throw new BusinessRuleException($"This appointment already has a treatment record.");
        }
        if (appointment.Status != AppointmentStatus.Scheduled)
        {
            throw new BusinessRuleException($"This appointment cannot be treated.");
        }
        var treatment = new Treatment(appointment.Id);
        try
        {
            await _treatmentRepository.Add(treatment, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            return treatment.Id;
        }
        catch (Exception)
        {
            await _unitOfWork.Rollback(cancellationToken);
            throw;
        }
    }
}
