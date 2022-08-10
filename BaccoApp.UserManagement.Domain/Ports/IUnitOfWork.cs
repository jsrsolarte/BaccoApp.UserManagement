using BaccoApp.UserManagement.Domain.Entities;

namespace BaccoApp.UserManagement.Domain.Ports;

public interface IUnitOfWork : IDisposable
{
    IRepository<T, TId> Repository<T, TId>() where T : EntityBase<TId>;
    int Complete();
}