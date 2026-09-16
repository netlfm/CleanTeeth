using AutoMapper;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using MediatR;

namespace CleanTeeth.Application.Features.Dentists.Queries.GetDentistDetail;

public class GetDentistDetailQueryHandler : IRequestHandler<GetDentistDetailQuery, GetDentistDetailResponse>
{
    private readonly IDentistRepository _repository;
    private readonly IMapper _mapper;

    public GetDentistDetailQueryHandler(IDentistRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<GetDentistDetailResponse> Handle(GetDentistDetailQuery request, CancellationToken cancellationToken)
    {
        var dentist = await _repository.GetById(request.Id, cancellationToken);
        if (dentist is null)
        {
            throw new NotFoundException($"Dentist with ID {request.Id} was not found.");
        }
        var dto = _mapper.Map<GetDentistDetailResponse>(dentist);
        return dto;
    }
}
