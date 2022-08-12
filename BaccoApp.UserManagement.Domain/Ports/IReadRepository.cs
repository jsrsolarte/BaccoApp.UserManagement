using Ardalis.Specification;
using BaccoApp.UserManagement.Domain.Entities.Base;

namespace BaccoApp.UserManagement.Domain.Ports;

public interface IReadRepository<T, in TId> where T : IEntityBase<TId>
{
    Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default);

    Task<T?> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    Task<TResult?> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> ListAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    Task<IEnumerable<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);
}