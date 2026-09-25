using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using MediatR;

namespace CleanTeeth.Application.Features.Patients.Commands.DeletePatient;

public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand>
{
    private readonly IPatientRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public DeletePatientCommandHandler(IPatientRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await _repository.GetById(request.Id, cancellationToken);
        if (patient is null)
        {
            throw new NotFoundException($"Patient with ID {request.Id} was not found.");
        }
        try
        {
            patient.Delete("admin");
            await _unitOfWork.Commit(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.Rollback(cancellationToken);
            throw;
        }
    }
}
