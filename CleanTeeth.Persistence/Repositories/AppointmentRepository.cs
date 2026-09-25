using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Features.Appointments.Queries.GetAppointmentList;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CleanTeeth.Persistence.Repositories;

public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    private readonly CleanTeethDbContext context;

    public AppointmentRepository(CleanTeethDbContext context)
        : base(context)
    {
        this.context = context;
    }

    public async Task<Appointment?> FindConflict(
        Guid patientId, Guid dentistId, Guid dentalOfficeId,
        DateTime start, DateTime end, CancellationToken cancellationToken)
    {
        return await context.Appointments
            .Where(x => x.Status == AppointmentStatus.Scheduled
                && start < x.TimeInterval.End && end > x.TimeInterval.Start
                && (x.DentistId == dentistId
                    || x.PatientId == patientId
                    || x.DentalOfficeId == dentalOfficeId))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public new async Task<Appointment?> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await context.Appointments
            .Include(x => x.Patient)
            .Include(x => x.Dentist)
            .Include(x => x.DentalOffice)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedResult<Appointment>> GetPagedAsync(int pageNumber, int pageSize, GetAppointmentListQuery filter, CancellationToken cancellationToken)
    {
        IQueryable<Appointment> query = context.Appointments
            .AsNoTracking()
            .Include(x => x.DentalOffice)
            .Include(x => x.Patient)
            .Include(x => x.Dentist);
        if (!string.IsNullOrWhiteSpace(filter.DentalOfficeName))
        {
            query = query.Where(x =>
                x.DentalOffice!.Name.Contains(filter.DentalOfficeName));
        }

        if (!string.IsNullOrWhiteSpace(filter.PatientName))
        {
            query = query.Where(x =>
                x.Patient!.Name.Contains(filter.PatientName));
        }

        if (!string.IsNullOrWhiteSpace(filter.DentistName))
        {
            query = query.Where(x =>
                x.Dentist!.Name.Contains(filter.DentistName));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.CreationTime)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Appointment>(items, totalCount, pageNumber, pageSize);
    }
}
