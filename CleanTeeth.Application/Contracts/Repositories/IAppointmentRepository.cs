using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Features.Appointments.Queries.GetAppointmentList;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Application.Contracts.Repositories;

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<Appointment?> FindConflict(Guid patientId, Guid dentistId, Guid dentalOfficeId, DateTime start, DateTime end, CancellationToken cancellationToken);
    new Task<Appointment?> GetById(Guid id, CancellationToken cancellationToken);
    Task<PagedResult<Appointment>> GetPagedAsync(
    int pageNumber,
    int pageSize,
    GetAppointmentListQuery Query,
    CancellationToken cancellationToken);
}
