using Ardalis.Specification;
using BaccoApp.UserManagement.Domain.Entities.Base;
using BaccoApp.UserManagement.Domain.Ports;
using BaccoApp.UserManagement.Infrastructure.Specification;

namespace BaccoApp.UserManagement.Infrastructure;

public class Repository<T> : ReadRepository<T>, IRepository<T> where T : DomainEntity
{
    private readonly PersistenceContext _context;

    public Repository(PersistenceContext context) : base(context, SpecificationEvaluator.Default)
    {
        _context = context;
    }

    public Repository(PersistenceContext context, ISpecificationEvaluator specificationEvaluator) : base(context,
        specificationEvaluator)
    {
        _context = context;
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        await _context.Set<T>().AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        var tasks = entities.Select(_ => AddAsync(_, cancellationToken));
        await Task.WhenAll(tasks);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().RemoveRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }
}