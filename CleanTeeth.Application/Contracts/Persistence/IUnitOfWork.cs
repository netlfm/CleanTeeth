namespace CleanTeeth.Application.Contracts.Persistence;

public interface IUnitOfWork
{
    Task Commit(CancellationToken cancellationToken);
    Task Rollback(CancellationToken cancellationToken);
}
