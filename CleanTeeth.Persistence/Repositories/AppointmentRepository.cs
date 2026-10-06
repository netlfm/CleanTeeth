using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Contracts.Security;
using CleanTeeth.Application.Features.Appointments.Queries.GetAppointmentList;
using CleanTeeth.Domain.Entities;
using CleanTeeth.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CleanTeeth.Persistence.Repositories;

public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    private readonly CleanTeethDbContext _context;
    private readonly IUserService _userService;
    public AppointmentRepository(CleanTeethDbContext context, IUserService userService)
        : base(context)
    {
        _context = context;
        _userService = userService;
    }

    public async Task<Appointment?> FindConflict(Guid patientId, Guid dentistId, Guid dentalOfficeId, DateTime start, DateTime end, CancellationToken cancellationToken)
    {
        return await _context.Appointments
            .Where(x => x.Status == AppointmentStatus.Scheduled
                && start < x.TimeInterval.End && end > x.TimeInterval.Start
                && (x.DentistId == dentistId
                    || x.PatientId == patientId
                    || x.DentalOfficeId == dentalOfficeId))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public new async Task<Appointment?> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = _context.Appointments
            .Include(x => x.Patient)
            .Include(x => x.Dentist)
            .Include(x => x.DentalOffice)
            .AsQueryable();
        if (_userService.IsInRole("Doctor"))
        {
            query = query.Where(x =>
                x.Dentist!.UserId == _userService.UserId);
        }
        else if (_userService.IsInRole("Patient"))
        {
            query = query.Where(x =>
                x.Patient!.UserId == _userService.UserId);
        }
        return await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedResult<Appointment>> GetPagedAsync(int pageNumber, int pageSize, GetAppointmentListQuery filter, CancellationToken cancellationToken)
    {
        IQueryable<Appointment> query = _context.Appointments
            .AsNoTracking()
            .Include(x => x.DentalOffice)
            .Include(x => x.Patient)
            .Include(x => x.Dentist);
        if (_userService.IsInRole("Doctor"))
        {
            query = query.Where(x =>
                x.Dentist!.UserId == _userService.UserId);
        }
        else if (_userService.IsInRole("Patient"))
        {
            query = query.Where(x =>
                x.Patient!.UserId == _userService.UserId);
        }
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
