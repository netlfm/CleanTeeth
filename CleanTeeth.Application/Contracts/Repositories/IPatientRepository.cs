using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Features.Patients.Queries.GetPatientList;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Application.Contracts.Repositories;

public interface IPatientRepository : IRepository<Patient>
{
    Task<PagedResult<Patient>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        GetPatientListQuery Query,
        CancellationToken cancellationToken = default);
}
