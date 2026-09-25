using AutoMapper;
using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Contracts.Repositories;
using MediatR;

namespace CleanTeeth.Application.Features.Appointments.Queries.GetAppointmentList;

public class GetAppointmentListQueryHandler : IRequestHandler<GetAppointmentListQuery, PagedResult<GetAppointmentListResponse>>
{
    private readonly IMapper _mapper;
    private readonly IAppointmentRepository _repository;
    public GetAppointmentListQueryHandler(IAppointmentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<PagedResult<GetAppointmentListResponse>> Handle(GetAppointmentListQuery request, CancellationToken cancellationToken)
    {
        var filter = new GetAppointmentListQuery
        {
            DentalOfficeName = request.DentalOfficeName,
            DentistName = request.DentistName,
            PatientName = request.PatientName,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Status = request.Status
        };
        var pageAppointment = await _repository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            filter,
            cancellationToken
            );
        var items = _mapper.Map<List<GetAppointmentListResponse>>(pageAppointment.Items);
        return new PagedResult<GetAppointmentListResponse>
            (items, pageAppointment.TotalCount, pageAppointment.PageNumber, pageAppointment.PageSize);
    }
}
