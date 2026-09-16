namespace CleanTeeth.Application.Contracts.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default);
    Task<T> Add(T entity, CancellationToken cancellationToken = default);
    Task Update(T entity, CancellationToken cancellationToken = default);
    Task Delete(T entity, CancellationToken cancellationToken = default);
}
