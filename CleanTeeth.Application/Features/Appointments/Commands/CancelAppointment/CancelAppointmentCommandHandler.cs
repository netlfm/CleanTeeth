using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using MediatR;

namespace CleanTeeth.Application.Features.Appointments.Commands.CancelAppointment;

public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand>
{
    private readonly IAppointmentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public CancelAppointmentCommandHandler(IAppointmentRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _repository.GetById(request.Id, cancellationToken);
        if (appointment is null)
        {
            throw new NotFoundException($"Appointment with ID {request.Id} was not found.");
        }
        appointment.Cancel();
        try
        {
            await _repository.Update(appointment,cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.Rollback(cancellationToken);
            throw;
        }
    }
}
