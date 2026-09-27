using AutoMapper;
using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Contracts.Repositories;
using MediatR;

namespace CleanTeeth.Application.Features.Treatments.Queries.GetTreatmentList;

public class GetTreatmentListQueryHandler : IRequestHandler<GetTreatmentListQuery, PagedResult<GetTreatmentListResponse>>
{
    private readonly ITreatmentRepository _repository;
    private readonly IMapper _mapper;

    public GetTreatmentListQueryHandler(ITreatmentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResult<GetTreatmentListResponse>> Handle(GetTreatmentListQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(request.PageNumber, request.PageSize, request, cancellationToken);

        var items = _mapper.Map<List<GetTreatmentListResponse>>(result.Items);
        return new PagedResult<GetTreatmentListResponse>(items, result.TotalCount, result.PageNumber, result.PageSize);
    }
}
