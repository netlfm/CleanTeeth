using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Features.Patients.Queries.GetPatientList;
using CleanTeeth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanTeeth.Persistence.Repositories;

public class PatientReository : Repository<Patient>, IPatientRepository
{
    private readonly CleanTeethDbContext _context;

    public PatientReository(CleanTeethDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<PagedResult<Patient>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        GetPatientListQuery filter,
        CancellationToken cancellationToken)
    {
        var query = _context.Patients.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(filter.PatientNumber))
        {
            query = query.Where(d => d.PatientNumber.Contains(filter.PatientNumber));
        }
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(d => d.Name != null && d.Name.Contains(filter.Name));
        }
        if (!string.IsNullOrWhiteSpace(filter.Phone))
        {
            query = query.Where(d => d.Phone.Value.Contains(filter.Phone));
        }
        if (!string.IsNullOrWhiteSpace(filter.Email))
        {
            query = query.Where(d => d.Email.Value.Contains(filter.Email));
        }
        if (filter.Gender.HasValue)
        {
            query = query.Where(x => x.Gender == filter.Gender);
        }
        if (filter.DateOfBirth.HasValue)
        {
            query = query.Where(x => x.DateOfBirth == filter.DateOfBirth);
        }

        if (!string.IsNullOrWhiteSpace(filter.Address))
        {
            query = query.Where(x => x.Address != null &&
                                     x.Address.Contains(filter.Address));
        }
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(d => d.CreationTime)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return new PagedResult<Patient>(items, totalCount, pageNumber, pageSize);
    }
}
