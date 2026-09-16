using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using MediatR;

namespace CleanTeeth.Application.Features.Dentists.Commands.OnLeaveDentist;

public class OnLeaveDentistCommandHandler : IRequestHandler<OnLeaveDentistCommand>
{
    private readonly IDentistRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public OnLeaveDentistCommandHandler(IDentistRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(OnLeaveDentistCommand request, CancellationToken cancellationToken)
    {
        var dentist = await _repository.GetById(request.Id, cancellationToken) ?? throw new NotFoundException($"Dentist with ID {request.Id} was not found.");
        dentist.ChangeStatus(Domain.Enums.DentistStatus.OnLeave);
        try
        {
            await _repository.Update(dentist, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.Rollback(cancellationToken);
            throw;
        }
    }
}
