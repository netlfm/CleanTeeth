using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Application.Contracts.Repositories;

public interface IDentalOfficeRepository : IRepository<DentalOffice>
{
    Task<IEnumerable<DentalOffice>> GetForCurrentUser(CancellationToken cancellationToken);
}
