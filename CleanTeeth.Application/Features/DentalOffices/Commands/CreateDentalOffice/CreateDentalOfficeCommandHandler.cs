using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.ValueObjects;
using MediatR;

namespace CleanTeeth.Application.Features.DentalOffices.Commands.CreateDentalOffice;

public class CreateDentalOfficeCommandHandler : IRequestHandler<CreateDentalOfficeCommand, Guid>
{
    private readonly IDentalOfficeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDentalOfficeCommandHandler(IDentalOfficeRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;

    }
    public async Task<Guid> Handle(CreateDentalOfficeCommand command, CancellationToken cancellationToken)
    {
        var dentalOffice = new DentalOffice(
                         command.Name,
                         command.Address,
                         new PhoneNumber(command.Phone),
                         new Email(command.Email));
        try
        {
            var result = await _repository.Add(dentalOffice, cancellationToken);
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
