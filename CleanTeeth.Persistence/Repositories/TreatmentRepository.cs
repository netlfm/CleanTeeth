using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Contracts.Security;
using CleanTeeth.Application.Features.Treatments.Queries.GetTreatmentList;
using CleanTeeth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanTeeth.Persistence.Repositories;

public class TreatmentRepository : Repository<Treatment>, ITreatmentRepository
{
    private readonly CleanTeethDbContext _context;
    private readonly IUserService _userService;

    public TreatmentRepository(CleanTeethDbContext context, IUserService userService) : base(context)
    {
        _context = context;
        _userService = userService;
    }
    public async Task<bool> ExistsByAppointmentId(Guid appointmentId, CancellationToken cancellationToken)
    {
        return await _context.Treatments
               .AnyAsync(x => x.AppointmentId == appointmentId, cancellationToken);
    }
    new public async Task<Treatment?> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = _context.Treatments
            .Include(x => x.Appointment)
            .ThenInclude(x => x!.Patient)
            .Include(x => x.Appointment)
            .ThenInclude(x => x!.Dentist)
            .Include(x => x.Appointment)
            .ThenInclude(x => x!.DentalOffice)
            .AsQueryable();
        if (_userService.IsInRole("Doctor"))
        {
            query = query.Where(x =>
                x.Appointment!.Dentist!.UserId == _userService.UserId);
        }
        else if (_userService.IsInRole("Patient"))
        {
            query = query.Where(x =>
                x.Appointment!.Patient!.UserId == _userService.UserId);
        }
        return await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedResult<Treatment>> GetPagedAsync(int pageNumber, int pageSize, GetTreatmentListQuery query, CancellationToken cancellationToken)
    {
        IQueryable<Treatment> treatments = _context.Treatments
               .AsNoTracking()
               .Include(x => x.Appointment)
                   .ThenInclude(x => x!.Patient)
               .Include(x => x.Appointment)
                   .ThenInclude(x => x!.Dentist)
               .Include(x => x.Appointment)
                   .ThenInclude(x => x!.DentalOffice);

        if (_userService.IsInRole("Doctor"))
        {
            treatments = treatments.Where(x =>
                x.Appointment!.Dentist!.UserId == _userService.UserId);
        }
        else if (_userService.IsInRole("Patient"))
        {
            treatments = treatments.Where(x =>
                x.Appointment!.Patient!.UserId == _userService.UserId);
        }

        if (!string.IsNullOrWhiteSpace(query.PatientName))
        {
            treatments = treatments.Where(x =>
                x.Appointment!.Patient!.Name.Contains(query.PatientName));
        }

        if (!string.IsNullOrWhiteSpace(query.DentistName))
        {
            treatments = treatments.Where(x =>
                x.Appointment!.Dentist!.Name.Contains(query.DentistName));
        }

        if (!string.IsNullOrWhiteSpace(query.DentalOfficeName))
        {
            treatments = treatments.Where(x =>
                x.Appointment!.DentalOffice!.Name.Contains(query.DentalOfficeName));
        }

        if (query.Status.HasValue)
        {
            treatments = treatments.Where(x =>
                x.Status == query.Status.Value);
        }
        if (query.StartDate.HasValue && query.EndDate.HasValue)
        {
            treatments = treatments.Where(x =>
                x.StartTime < query.EndDate.Value &&
                x.EndTime > query.StartDate.Value);
        }

        var totalCount = await treatments.CountAsync(cancellationToken);

        var items = await treatments
            .OrderByDescending(x => x.CreationTime)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Treatment>(
            items,
            totalCount,
            pageNumber,
            pageSize);
    }
}
