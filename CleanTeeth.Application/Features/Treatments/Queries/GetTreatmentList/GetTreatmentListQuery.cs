using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Domain.Enums;
using MediatR;

namespace CleanTeeth.Application.Features.Treatments.Queries.GetTreatmentList;

public class GetTreatmentListQuery : IRequest<PagedResult<GetTreatmentListResponse>>
{
    public string? PatientName { get; init; }
    public string? DentistName { get; init; }
    public string? DentalOfficeName { get; init; }

    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }

    public TreatmentStatus? Status { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}
