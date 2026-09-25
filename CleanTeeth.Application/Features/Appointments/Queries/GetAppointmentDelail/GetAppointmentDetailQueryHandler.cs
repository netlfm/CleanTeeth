using AutoMapper;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Exceptions;
using MediatR;

namespace CleanTeeth.Application.Features.Appointments.Queries.GetAppointmentDelail;

public class GetAppointmentDetailQueryHandler : IRequestHandler<GetAppointmentDetailQuery, GetAppointmentDetailResponse>
{
    private readonly IAppointmentRepository _repository;
    private readonly IMapper _mapper;

    public GetAppointmentDetailQueryHandler(IAppointmentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<GetAppointmentDetailResponse> Handle(GetAppointmentDetailQuery request, CancellationToken cancellationToken)
    {
        var appointment = await _repository.GetById(request.Id, cancellationToken);
        if (appointment is null)
        {
            throw new NotFoundException($"Appointment with ID {request.Id} was not found.");

        }
        var reuslt = _mapper.Map<GetAppointmentDetailResponse>(appointment);
        return reuslt;
    }
}
