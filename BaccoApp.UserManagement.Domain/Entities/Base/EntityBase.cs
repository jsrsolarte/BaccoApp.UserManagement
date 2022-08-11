namespace BaccoApp.UserManagement.Domain.Entities.Base;

public abstract class EntityBase<T> : DomainEntity, IEntityBase<T>
{
    public virtual T Id { get; init; } = default!;
}