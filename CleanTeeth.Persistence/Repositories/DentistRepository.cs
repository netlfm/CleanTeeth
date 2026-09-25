using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Features.Dentists.Queries.GetDentistList;
using CleanTeeth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanTeeth.Persistence.Repositories;

public class DentistRepository : Repository<Dentist>, IDentistRepository
{
    private readonly CleanTeethDbContext _context;

    public DentistRepository(CleanTeethDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<PagedResult<Dentist>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        GetDentistListQuery filter,
        CancellationToken cancellationToken)
    {
        var query = _context.Dentists.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(d => d.Name.Contains(filter.Name));
        }
        if (!string.IsNullOrWhiteSpace(filter.Specialty))
        {
            query = query.Where(d => d.Specialty != null && d.Specialty.Contains(filter.Specialty));
        }
        if (filter.Status.HasValue)
        {
            query = query.Where(d => d.Status == filter.Status.Value);
        }
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(d => d.CreationTime)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return new PagedResult<Dentist>(items, totalCount, pageNumber, pageSize);
    }
}
