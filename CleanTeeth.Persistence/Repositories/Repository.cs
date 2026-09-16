using CleanTeeth.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CleanTeeth.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly CleanTeethDbContext context;
    public Repository(CleanTeethDbContext context)
    {
        this.context = context;
    }
    public Task<T> Add(T entity, CancellationToken cancellationToken)
    {
        context.Add(entity);
        return Task.FromResult(entity);
    }

    public Task Delete(T entity, CancellationToken cancellationToken)
    {
        context.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<T?> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await context.Set<T>().FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
    }

    public Task Update(T entity, CancellationToken cancellationToken)
    {
        context.Update(entity);
        return Task.CompletedTask;
    }
}
