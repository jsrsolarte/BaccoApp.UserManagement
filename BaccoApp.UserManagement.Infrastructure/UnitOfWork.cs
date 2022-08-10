using BaccoApp.UserManagement.Domain.Entities;
using BaccoApp.UserManagement.Domain.Ports;

namespace BaccoApp.UserManagement.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly PersistenceContext _context;
    private Dictionary<string, dynamic>? _repositories;

    public UnitOfWork(PersistenceContext context)
    {
        _context = context;
    }

    public int Complete()
    {
        return _context.SaveChanges();
    }

    public IRepository<T, TId> Repository<T, TId>() where T : EntityBase<TId>
    {
        _repositories ??= new Dictionary<string, dynamic>();
        var type = typeof(T).Name;
        if (_repositories.ContainsKey(type))
            return (IRepository<T, TId>)_repositories[type];
        var repositoryType = typeof(Repository<T, TId>);
        var instance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), this);

        if (instance != null) _repositories.Add(type, instance);
        return _repositories[type];
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    private void ReleaseUnmanagedResources()
    {
        _context.Dispose();
    }

    private void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
        if (disposing) _context.Dispose();
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }
}