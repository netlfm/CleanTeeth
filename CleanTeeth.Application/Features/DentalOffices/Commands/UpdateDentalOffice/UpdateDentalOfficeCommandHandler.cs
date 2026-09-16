using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Domain.ValueObjects;
using MediatR;

namespace CleanTeeth.Application.Features.DentalOffices.Commands.UpdateDentalOffice;

public class UpdateDentalOfficeCommandHandler : IRequestHandler<UpdateDentalOfficeCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDentalOfficeRepository _repository;
    public UpdateDentalOfficeCommandHandler(IUnitOfWork unitOfWork, IDentalOfficeRepository repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }
    public async Task Handle(UpdateDentalOfficeCommand request, CancellationToken cancellationToken)
    {
        var dentaloffice = await _repository.GetById(request.Id, cancellationToken);
        if (dentaloffice is null)
        {
            throw new NotFoundException($"Dental office with ID {request.Id} was not found.");
        }
        var email = new Email(request.Email);
        var phoneNumber = new PhoneNumber(request.Phone);
        dentaloffice.Update(request.Name, request.Address, phoneNumber, email);
        try
        {
            await _repository.Update(dentaloffice, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.Rollback(cancellationToken);
            throw;
        }
    }
}
