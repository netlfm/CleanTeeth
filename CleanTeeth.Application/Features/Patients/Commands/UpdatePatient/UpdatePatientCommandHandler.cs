using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using CleanTeeth.Domain.ValueObjects;
using MediatR;

namespace CleanTeeth.Application.Features.Patients.Commands.UpdatePatient;

public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand>
{
    private readonly IPatientRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePatientCommandHandler(IPatientRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await _repository.GetById(request.Id, cancellationToken);
        if (patient is null)
        {
            throw new NotFoundException($"Patient with ID {request.Id} was not found.");
        }
        var email = new Email(request.Email);
        var phone = new PhoneNumber(request.Phone);
        patient.Update(request.Name, request.DateOfBirth, request.Gender, phone, email, request.Address);
        try
        {
            await _repository.Update(patient, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.Rollback(cancellationToken);
            throw;
        }
    }
}
