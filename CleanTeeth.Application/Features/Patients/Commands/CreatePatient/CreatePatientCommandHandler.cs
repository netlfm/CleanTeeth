using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;
using MediatR;

namespace CleanTeeth.Application.Features.Patients.Commands.CreatePatient;

public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Guid>
{
    private readonly IPatientRepository _reository;
    private readonly IUnitOfWork _unitOfWork;
    public CreatePatientCommandHandler(IPatientRepository reository, IUnitOfWork unitOfWork)
    {
        _reository = reository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        var patientNumber = $"P{DateTime.UtcNow:yyyyMMddHHmmssfff}";
        var patient = new Patient(
            request.Name,
            patientNumber,
            request.DateOfBirth,
            request.Gender,
            new PhoneNumber(request.Phone),
            new Email(request.Email),
            request.Address
            );
        try
        {
            var result = await _reository.Add(patient, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            return result.Id;
        }
        catch (Exception)
        {
            await _unitOfWork.Rollback(cancellationToken);
            throw;
        }
    }
}
