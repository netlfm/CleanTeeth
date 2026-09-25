namespace CleanTeeth.Application.Contracts.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetById(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken);
    Task<T> Add(T entity, CancellationToken cancellationToken);
    Task Update(T entity, CancellationToken cancellationToken);
    Task Delete(T entity, CancellationToken cancellationToken);
}
