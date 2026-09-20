using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Features.Dentists.Queries.GetDentistList;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Application.Contracts.Repositories;

public interface IDentistRepository : IRepository<Dentist>
{
    Task<PagedResult<Dentist>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        GetDentistListQuery Query,
        CancellationToken cancellationToken = default);
}
