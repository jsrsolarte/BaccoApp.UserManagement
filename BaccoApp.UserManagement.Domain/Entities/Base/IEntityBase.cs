﻿namespace BaccoApp.UserManagement.Domain.Entities.Base;

public interface IEntityBase<out T>
{
    T Id { get; }
}