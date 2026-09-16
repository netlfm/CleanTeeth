using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Domain.ValueObjects;
using MediatR;

namespace CleanTeeth.Application.Features.Dentists.Commands.UpdateDentist;

public class UpdateDentistCommandHandler : IRequestHandler<UpdateDentistCommand>
{
    private readonly IDentistRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateDentistCommandHandler(IDentistRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(UpdateDentistCommand request, CancellationToken cancellationToken)
    {
        var dentist = await _repository.GetById(request.Id, cancellationToken);
        if (dentist is null)
        {
            throw new NotFoundException($"Dentist with ID {request.Id} was not found.");
        }
        var email = new Email(request.Email);
        var phone = new PhoneNumber(request.Phone);
        dentist.Update(request.Name, request.Gender, phone, email, request.LicenseNumber, request.Specialty);
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
