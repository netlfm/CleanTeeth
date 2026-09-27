using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Features.Treatments.Queries.GetTreatmentList;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Application.Contracts.Repositories;

public interface ITreatmentRepository : IRepository<Treatment>
{
    public Task<bool> ExistsByAppointmentId(Guid AppointmentId, CancellationToken cancellationToken);
    new public Task<Treatment?> GetById(Guid id, CancellationToken cancellationToken);
    Task<PagedResult<Treatment>> GetPagedAsync(int pageNumber, int pageSize, GetTreatmentListQuery Query, CancellationToken cancellationToken);
}
