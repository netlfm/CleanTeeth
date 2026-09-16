namespace CleanTeeth.Application.Contracts.Persistence;

public interface IUnitOfWork
{
    Task Commit(CancellationToken cancellationToken = default);
    Task Rollback(CancellationToken cancellationToken = default);
}
