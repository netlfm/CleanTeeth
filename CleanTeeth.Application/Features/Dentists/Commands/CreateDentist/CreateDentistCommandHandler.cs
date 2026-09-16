using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;
using MediatR;

namespace CleanTeeth.Application.Features.Dentists.Commands.CreateDentist;

public class CreateDentistCommandHandler : IRequestHandler<CreateDentistCommand, Guid>
{
    private readonly IDentistRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateDentistCommandHandler(IDentistRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Guid> Handle(CreateDentistCommand request, CancellationToken cancellationToken)
    {
        var dentist = new Dentist(
                    request.Name,
                    request.Gender,
                    new PhoneNumber(request.Phone),
                    new Email(request.Email),
                    request.LicenseNumber,
                    request.Specialty);
        try
        {
            var result = await _repository.Add(dentist, cancellationToken);
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
