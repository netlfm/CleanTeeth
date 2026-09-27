using AutoMapper;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using MediatR;

namespace CleanTeeth.Application.Features.Treatments.Queries.GetTreatmentDetail;

public class GetTreatmentDetailQueryHandler : IRequestHandler<GetTreatmentDetailQuery, GetTreatmentDetailResponse>
{
    private readonly ITreatmentRepository _repository;
    private readonly IMapper _mapper;
    public GetTreatmentDetailQueryHandler(ITreatmentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<GetTreatmentDetailResponse> Handle(GetTreatmentDetailQuery request, CancellationToken cancellationToken)
    {
        var treatment = await _repository.GetById(request.Id, cancellationToken);
        if (treatment is null)
        {
            throw new NotFoundException($"Treatment with ID {request.Id} was not found.");
        }
        return _mapper.Map<GetTreatmentDetailResponse>(treatment);
    }
}
