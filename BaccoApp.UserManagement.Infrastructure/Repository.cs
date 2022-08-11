using Ardalis.Specification;
using BaccoApp.UserManagement.Domain.Entities;
using BaccoApp.UserManagement.Domain.Entities.Base;
using BaccoApp.UserManagement.Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace BaccoApp.UserManagement.Infrastructure;

public class Repository<T, TId> : ReadRepository<T, TId>, IRepository<T, TId> where T : EntityBase<TId>
{
    private readonly DbContext _dbContext;

    public Repository(DbContext dbContext, ISpecificationEvaluator specificationEvaluator) : base(dbContext,
        specificationEvaluator)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
    }

    public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        var tasks = entities.Select(_ => AddAsync(_, cancellationToken));
        return Task.WhenAll(tasks);
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        _dbContext.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().RemoveRange(entities);
        return Task.CompletedTask;
    }
}