using AutoMapper;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using MediatR;

namespace CleanTeeth.Application.Features.Patients.Queries.GetPatientDetail;

public class GetPatientDetailQueryHandler : IRequestHandler<GetPatientDetailQuery, GetPatientDetailResponse>
{
    private readonly IPatientRepository _repository;
    private readonly IMapper _mapper;
    public GetPatientDetailQueryHandler(IPatientRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<GetPatientDetailResponse> Handle(GetPatientDetailQuery request, CancellationToken cancellationToken)
    {
        var patient = await _repository.GetById(request.Id, cancellationToken);
        if (patient is null)
        {
            throw new NotFoundException($"Paitnet with ID {request.Id} was not found.");
        }
        return _mapper.Map<GetPatientDetailResponse>(patient);
    }
}
