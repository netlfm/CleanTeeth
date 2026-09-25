using AutoMapper;
using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Contracts.Repositories;
using MediatR;

namespace CleanTeeth.Application.Features.Patients.Queries.GetPatientList;

public class GetPatientListQueryHandler : IRequestHandler<GetPatientListQuery, PagedResult<GetPatientListResponse>>
{
    private readonly IMapper _mapper;
    private readonly IPatientRepository _repository;
    public GetPatientListQueryHandler(IPatientRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<PagedResult<GetPatientListResponse>> Handle(GetPatientListQuery request, CancellationToken cancellationToken)
    {
        var filter = new GetPatientListQuery
        {
            PatientNumber = request.PatientNumber,
            Name = request.Name,
            Gender = request.Gender,
            DateOfBirth = request.DateOfBirth,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address
        };
        var pagedPatient = await _repository.GetPagedAsync(
         request.PageNumber,
         request.PageSize,
         filter,
         cancellationToken);
        var items = _mapper.Map<List<GetPatientListResponse>>(pagedPatient.Items);
        return new PagedResult<GetPatientListResponse>
            (items, pagedPatient.TotalCount, pagedPatient.PageNumber, pagedPatient.PageSize);
    }
}
