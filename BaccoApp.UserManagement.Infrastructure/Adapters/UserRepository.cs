using BaccoApp.UserManagement.Domain.Entities;
using BaccoApp.UserManagement.Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace BaccoApp.UserManagement.Infrastructure.Adapters
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(PersistenceContext context) : base(context)
        {
        }
    }
}