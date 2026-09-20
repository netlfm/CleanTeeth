using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using MediatR;

namespace CleanTeeth.Application.Features.Dentists.Commands.DeleteDentist;

public class DeleteDentistCommandHandler : IRequestHandler<DeleteDentistCommand>
{
    private readonly IDentistRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteDentistCommandHandler(IDentistRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(DeleteDentistCommand request, CancellationToken cancellationToken)
    {
        var dentist = await _repository.GetById(request.Id, cancellationToken) ?? throw new NotFoundException($"Dentist with ID {request.Id} was not found.");
        try
        {
            dentist.Delete("admin");
            await _unitOfWork.Commit(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.Rollback(cancellationToken);
            throw;
        }
    }
}
