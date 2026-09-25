using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using MediatR;

namespace CleanTeeth.Application.Features.Appointments.Commands.CompleteAppointment;

public class CompleteAppointmentCommandHandler : IRequestHandler<CompleteAppointmentCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppointmentRepository _repository;
    public CompleteAppointmentCommandHandler(IAppointmentRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(CompleteAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _repository.GetById(request.Id, cancellationToken);
        if(appointment is null)
        {
            throw new NotFoundException($"Appointment with ID {request.Id} was not found.");
        }
        appointment.Complete();
        try
        {
            await _repository.Update(appointment, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
        }
        catch (Exception)
        {
           await _unitOfWork.Rollback(cancellationToken);
            throw;
        }
    }
}
