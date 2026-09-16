using AutoMapper;
using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Contracts.Repositories;
using MediatR;

namespace CleanTeeth.Application.Features.Dentists.Queries.GetDentistList;

public class GetDentistListQueryHandler : IRequestHandler<GetDentistListQuery, PagedResult<GetDentistListReponse>>
{
    private readonly IDentistRepository _repository;
    private readonly IMapper _mapper;
    public GetDentistListQueryHandler(IDentistRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<PagedResult<GetDentistListReponse>> Handle(GetDentistListQuery request, CancellationToken cancellationToken)
    {
        var filter = new DentistQueryFilter
        {
            Name = request.Name,
            Specialty = request.Specialty,
            Status = request.Status,
            CreatedFrom = request.CreatedFrom,
            CreatedTo = request.CreatedTo
        };
        var pagedDentists = await _repository.GetPagedAsync(
           request.PageNumber ?? 1,
           request.PageSize ?? 10,
           filter,
           cancellationToken);

        var items = _mapper.Map<List<GetDentistListReponse>>(pagedDentists.Items);
        return new PagedResult<GetDentistListReponse>
            (items, pagedDentists.TotalCount, pagedDentists.PageNumber, pagedDentists.PageSize);
    }
}
