using BaccoApp.UserManagement.Domain.Entities;
using BaccoApp.UserManagement.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BaccoApp.UserManagement.Infrastructure;

public class PersistenceContext : DbContext
{
    private readonly IConfiguration _config;

    public PersistenceContext(DbContextOptions<PersistenceContext> options, IConfiguration config) : base(options)
    {
        _config = config;
    }

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.HasDefaultSchema(_config.GetValue<string>("Database:SchemaName"));

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.AddProperty("CreatedDate", typeof(DateTime));
            entity.AddProperty("UpdatedDate", typeof(DateTime));
        }

        modelBuilder.Seed();
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e =>
                e.State is EntityState.Added or EntityState.Modified);

        foreach (var entityEntry in entries)
        {
            entityEntry.Property("UpdatedDate").CurrentValue = DateTime.Now;

            if (entityEntry.State == EntityState.Added) entityEntry.Property("CreatedDate").CurrentValue = DateTime.Now;
        }

        return base.SaveChanges();
    }
}