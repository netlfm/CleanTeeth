using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Domain.Enums;
using MediatR;

namespace CleanTeeth.Application.Features.Dentists.Queries.GetDentistList;

public class GetDentistListQuery : IRequest<PagedResult<GetDentistListReponse>>
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public string? Name { get; set; }
    public string? Specialty { get; set; }
    public DentistStatus? Status { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
}