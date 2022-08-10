using Microsoft.EntityFrameworkCore;

namespace BaccoApp.UserManagement.Infrastructure;

public class PersistenceContext : DbContext
{
    public PersistenceContext(DbContextOptions<PersistenceContext> options) : base(options)
    {
    }
}