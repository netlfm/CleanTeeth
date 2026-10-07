using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Contracts.Security;
using CleanTeeth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanTeeth.Persistence.Repositories;

public class DentalOfficeRepository : Repository<DentalOffice>, IDentalOfficeRepository
{
    private readonly CleanTeethDbContext _context;
    private readonly IUserService _userService;

    public DentalOfficeRepository(CleanTeethDbContext context, IUserService userService)
        : base(context)
    {
        _context = context;
        _userService = userService;
    }

    public async Task<IEnumerable<DentalOffice>> GetForCurrentUser(CancellationToken cancellationToken)
    {
        var query = ApplyOwnershipFilter(_context.DentalOffices.AsNoTracking());
        return await query.OrderBy(x => x.Name).ToListAsync(cancellationToken);
    }

    new public async Task<DentalOffice?> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = ApplyOwnershipFilter(_context.DentalOffices.AsQueryable());
        return await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    private IQueryable<DentalOffice> ApplyOwnershipFilter(IQueryable<DentalOffice> query)
    {
        if (_userService.IsInRole("Dentist"))
        {
            return query.Where(x => x.CreatedBy == _userService.UserId);
        }
        if (_userService.IsInRole("Patient"))
        {
            var officeIds = _context.Appointments
                .Where(a => a.Patient!.UserId == _userService.UserId)
                .Select(a => a.DentalOfficeId)
                .Distinct();
            return query.Where(x => officeIds.Contains(x.Id));
        }
        return query;
    }
}
