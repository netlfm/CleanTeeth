using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Notifications;
using CleanTeeth.Domain.Exceptions;
using MediatR;

namespace CleanTeeth.Application.Features.Treatments.Command.CompleteTreatment;

public class CompleteTreatmentCommandHandler : IRequestHandler<CompleteTreatmentCommand>
{
    private readonly ITreatmentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotifications _notifications;
    public CompleteTreatmentCommandHandler(ITreatmentRepository repository, IUnitOfWork unitOfWork, INotifications notifications)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _notifications = notifications;
    }
    public async Task Handle(CompleteTreatmentCommand request, CancellationToken cancellationToken)
    {
        var treatment = await _repository.GetById(request.Id, cancellationToken);
        if (treatment is null)
        {
            throw new BusinessRuleException($"Treatment does not exist.");
        }
        var endtime = DateTime.UtcNow;
        treatment.CompleteTreatment(endtime, request.Notes);
        try
        {
            await _repository.Update(treatment, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.Rollback(cancellationToken);
            throw;
        }
        var dto = new TreatmentReportDTO
        {
            Id = treatment.Id,
            DentalOffice = treatment.Appointment!.DentalOffice!.Name,
            Dentist = treatment.Appointment.Dentist!.Name,
            Patient = treatment.Appointment.Patient!.Name,
            PatientPhone = treatment.Appointment.Patient.Phone.Value,
            Date = treatment.StartTime!.Value,
            DurationMinutes = treatment.DurationMinutes,
            Notes = treatment.Notes,
            CompletedAt = treatment.EndTime!.Value
        };
        await _notifications.SendTreatmentReport(dto);
    }
}
