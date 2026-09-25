using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Domain.Enums;
using MediatR;

namespace CleanTeeth.Application.Features.Dentists.Queries.GetDentistList;

public class GetDentistListQuery : IRequest<PagedResult<GetDentistListResponse>>
{
    public string? Name { get; init; }
    public string? Specialty { get; init; }
    public DentistStatus? Status { get; init; }

    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}