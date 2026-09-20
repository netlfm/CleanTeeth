using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Domain.Enums;
using MediatR;

namespace CleanTeeth.Application.Features.Patients.Queries.GetPatientList;

public class GetPatientListQuery : IRequest<PagedResult<GetPatientListReponse>>
{
    public string? PatientNumber { get; init; }
    public string? Name { get; init; }
    public Gender? Gender { get; init; }
    public DateOnly? DateOfBirth { get; init; }
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public string? Address { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}
