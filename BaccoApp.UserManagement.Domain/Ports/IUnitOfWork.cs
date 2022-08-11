using BaccoApp.UserManagement.Domain.Entities;
using BaccoApp.UserManagement.Domain.Entities.Base;

namespace BaccoApp.UserManagement.Domain.Ports;

public interface IUnitOfWork : IDisposable
{
    IRepository<T, TId> Repository<T, TId>() where T : EntityBase<TId>;
    int Complete();
}