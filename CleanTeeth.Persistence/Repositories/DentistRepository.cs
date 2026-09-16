using CleanTeeth.Application.Contracts.Common.Model;
using CleanTeeth.Application.Contracts.Repositories;
using CleanTeeth.Application.Features.Dentists.Queries;
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
        DentistQueryFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Dentists.AsNoTracking();

        // 动态拼接 Where 条件
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

        if (filter.CreatedFrom.HasValue)
        {
            query = query.Where(d => d.CreationTime >= filter.CreatedFrom.Value);
        }

        if (filter.CreatedTo.HasValue)
        {
            query = query.Where(d => d.CreationTime <= filter.CreatedTo.Value);
        }

        // Count 和列表查询共用同一个 query，保证过滤条件一致
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(d => d.CreationTime)   // 按创建时间倒序，最新的在前
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Dentist>(items, totalCount, pageNumber, pageSize);
    }
}
