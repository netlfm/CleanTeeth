using CleanTeeth.Application.Contracts.Persistence;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Domain.Exceptions;
using MediatR;

namespace CleanTeeth.Application.Features.Treatments.Command.CompleteTreatment;

public class CompleteTreatmentCommandHandler : IRequestHandler<CompleteTreatmentCommand>
{
    private readonly ITreatmentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public CompleteTreatmentCommandHandler(ITreatmentRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(CompleteTreatmentCommand request, CancellationToken cancellationToken)
    {
        var treatment = await _repository.GetById(request.Id, cancellationToken);
        if (treatment is null)
        {
            throw new BusinessRuleException($"Treatment does not exist.");
        }
        var endtime = DateTime.UtcNow;
        treatment.CompleteTreatment(endtime, request.Notes);
        try
        {
            await _repository.Update(treatment, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.Rollback(cancellationToken);
            throw;
        }
    }
}
