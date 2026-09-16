using AutoMapper;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using MediatR;

namespace CleanTeeth.Application.Features.DentalOffices.Queries.GetDentalOfficeDetail;

public class GetDentalOfficeDetailQueryHandler : IRequestHandler<GetDentalOfficeDetailQuery, GetDentalOfficeDetailResponse>
{
    private readonly IDentalOfficeRepository _repository;
    private readonly IMapper _mapper;

    public GetDentalOfficeDetailQueryHandler(IDentalOfficeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<GetDentalOfficeDetailResponse> Handle(GetDentalOfficeDetailQuery request, CancellationToken cancellationToken)
    {
        var dentalOffice = await _repository.GetById(request.Id, cancellationToken);

        if (dentalOffice is null)
        {
            throw new NotFoundException(
                  $"Dental office with ID {request.Id} was not found.");
        }
        var dto = _mapper.Map<GetDentalOfficeDetailResponse>(dentalOffice);
        return dto;
    }
}
