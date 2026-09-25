using AutoMapper;
using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Contracts.Repositories;
using MediatR;

namespace CleanTeeth.Application.Features.Dentists.Queries.GetDentistList;

public class GetDentistListQueryHandler : IRequestHandler<GetDentistListQuery, PagedResult<GetDentistListResponse>>
{
    private readonly IDentistRepository _repository;
    private readonly IMapper _mapper;
    public GetDentistListQueryHandler(IDentistRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<PagedResult<GetDentistListResponse>> Handle(GetDentistListQuery request, CancellationToken cancellationToken)
    {
        var filter = new GetDentistListQuery
        {
            Name = request.Name,
            Specialty = request.Specialty,
            Status = request.Status
        };
        var pagedDentists = await _repository.GetPagedAsync(
           request.PageNumber,
           request.PageSize,
           filter,
           cancellationToken);

        var items = _mapper.Map<List<GetDentistListResponse>>(pagedDentists.Items);
        return new PagedResult<GetDentistListResponse>
            (items, pagedDentists.TotalCount, pagedDentists.PageNumber, pagedDentists.PageSize);
    }
}
