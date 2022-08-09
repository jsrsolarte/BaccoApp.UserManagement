namespace BaccoApp.UserManagement.Domain.Entities;

public interface IEntityBase<out T>
{
    T Id { get; }
}