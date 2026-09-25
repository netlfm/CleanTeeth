using CleanTeeth.Application.Contracts.Common.Model;
using MediatR;

namespace CleanTeeth.Application.Features.Appointments.Queries.GetAppointmentList;

public class GetAppointmentListQuery : IRequest<PagedResult<GetAppointmentListResponse>>
{
    public string? PatientName { get; init; }
    public string? DentistName { get; init; }
    public string? DentalOfficeName { get; init; }
    public string? Status { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}
