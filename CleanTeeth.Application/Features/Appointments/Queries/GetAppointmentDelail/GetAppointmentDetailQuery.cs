using MediatR;

namespace CleanTeeth.Application.Features.Appointments.Queries.GetAppointmentDelail;

public class GetAppointmentDetailQuery : IRequest<GetAppointmentDetailResponse>
{
    public required Guid Id { get; set; }
}
