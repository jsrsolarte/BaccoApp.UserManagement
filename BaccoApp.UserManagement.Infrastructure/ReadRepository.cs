using Ardalis.Specification;
using BaccoApp.UserManagement.Domain.Entities.Base;
using BaccoApp.UserManagement.Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace BaccoApp.UserManagement.Infrastructure;

public class ReadRepository<T> : IReadRepository<T> where T : DomainEntity
{
    private readonly PersistenceContext _context;
    private readonly ISpecificationEvaluator _specificationEvaluator;

    protected ReadRepository(PersistenceContext context, ISpecificationEvaluator specificationEvaluator)
    {
        _context = context;
        _specificationEvaluator = specificationEvaluator;
    }

    public async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().FindAsync(new[] { id }, cancellationToken);
    }

    public async Task<T?> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TResult?> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().ToListAsync(cancellationToken);
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
        return await _context.Set<T>().CountAsync(cancellationToken);
    }

    protected virtual IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
    {
        if (specification is null) throw new ArgumentNullException(nameof(specification));
        if (specification.Selector is null) throw new SelectorNotFoundException();

        return _specificationEvaluator.GetQuery(_context.Set<T>().AsQueryable(), specification);
    }

    protected virtual IQueryable<T> ApplySpecification(ISpecification<T> specification,
        bool evaluateCriteriaOnly = false)
    {
        if (specification is null) throw new ArgumentNullException(nameof(specification));
        return _specificationEvaluator.GetQuery(_context.Set<T>().AsQueryable(), specification, evaluateCriteriaOnly);
    }
}