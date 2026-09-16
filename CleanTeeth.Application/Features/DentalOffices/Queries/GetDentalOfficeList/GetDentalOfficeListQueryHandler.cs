using AutoMapper;
using CleanTeeth.Application.Contracts.Repositories;
using MediatR;

namespace CleanTeeth.Application.Features.DentalOffices.Queries.GetDentalOfficeList;

public class GetDentalOfficeListQueryHandler : IRequestHandler<GetDentalOfficeListQuery, List<GetDentalOfficeListResponse>>
{
    private readonly IDentalOfficeRepository _repository;
    private readonly IMapper _mapper;
    public GetDentalOfficeListQueryHandler(IDentalOfficeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<List<GetDentalOfficeListResponse>> Handle(GetDentalOfficeListQuery request, CancellationToken cancellationToken)
    {
        var dentaloffice = await _repository.GetAll(cancellationToken);
        var items = _mapper.Map<List<GetDentalOfficeListResponse>>(dentaloffice);
        return items;
    }
}
