using BaccoApp.UserManagement.Domain.Entities.Base;

namespace BaccoApp.UserManagement.Domain.Entities;

public class User : EntityBase<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}