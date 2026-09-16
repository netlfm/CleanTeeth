using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using MediatR;

namespace CleanTeeth.Application.Features.DentalOffices.Commands.DeleteDentalOffice;

public class DeleteDentalOfficeCommandHandler : IRequestHandler<DeleteDentalOfficeCommand>
{
    private readonly IDentalOfficeRepository _repository;
    private readonly IUnitOfWork _unitofwork;
    public DeleteDentalOfficeCommandHandler(IDentalOfficeRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitofwork = unitOfWork;
    }
    public async Task Handle(DeleteDentalOfficeCommand request, CancellationToken cancellationToken)
    {
        var dentaloffice = await _repository.GetById(request.Id, cancellationToken);
        if (dentaloffice is null)
        {
            throw new NotFoundException($"Dental office with ID {request.Id} was not found.");
        }
        try
        {
            await _repository.Delete(dentaloffice, cancellationToken);
            await _unitofwork.Commit(cancellationToken);
        }
        catch (Exception)
        {
            await _unitofwork.Rollback(cancellationToken);
            throw;
        }
    }
}
