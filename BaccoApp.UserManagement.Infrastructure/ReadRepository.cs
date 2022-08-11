using Ardalis.Specification;
using BaccoApp.UserManagement.Domain.Entities;
using BaccoApp.UserManagement.Domain.Entities.Base;
using BaccoApp.UserManagement.Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace BaccoApp.UserManagement.Infrastructure;

public class ReadRepository<T, TId> : IReadRepository<T, TId> where T : EntityBase<TId>
{
    private readonly DbContext _dbContext;
    private readonly ISpecificationEvaluator _specificationEvaluator;

    protected ReadRepository(DbContext dbContext, ISpecificationEvaluator specificationEvaluator)
    {
        _dbContext = dbContext;
        _specificationEvaluator = specificationEvaluator;
    }

    public async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().FindAsync(new[] { id }, cancellationToken: cancellationToken);
    }

    public async Task<T?> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstAsync(cancellationToken);
    }

    public async Task<TResult?> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> ListAsync(ISpecification<T> specification,
        CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).CountAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().CountAsync(cancellationToken);
    }

    protected virtual IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
    {
        if (specification is null) throw new ArgumentNullException(nameof(specification));
        if (specification.Selector is null) throw new SelectorNotFoundException();

        return _specificationEvaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), specification);
    }

    protected virtual IQueryable<T> ApplySpecification(ISpecification<T> specification,
        bool evaluateCriteriaOnly = false)
    {
        if (specification is null) throw new ArgumentNullException(nameof(specification));
        return _specificationEvaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), specification, evaluateCriteriaOnly);
    }
}