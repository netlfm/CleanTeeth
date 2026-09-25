using CleanTeeth.Application.Contracts.Persistence;

namespace CleanTeeth.Persistence.UnitsOfWork;

public class UnitOfWorkEFCore : IUnitOfWork
{
    private readonly CleanTeethDbContext _context;
    public UnitOfWorkEFCore(CleanTeethDbContext context)
    {
        _context = context;
    }
    public async Task Commit(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task Rollback(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
