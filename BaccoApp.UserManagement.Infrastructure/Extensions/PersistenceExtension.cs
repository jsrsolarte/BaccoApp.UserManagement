using BaccoApp.UserManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaccoApp.UserManagement.Infrastructure.Extensions;

public static class PersistenceExtension
{
    public static IServiceCollection AddPersistence(this IServiceCollection svc, IConfiguration config)
    {
        var connectionString = config.GetValue<string>("Database:ConnectionString");
        var schemaName = config.GetValue<string>("Database:SchemaName");

        svc.AddDbContext<PersistenceContext>(_ => _.UseSqlServer(connectionString, optionsBuilder =>
        {
            optionsBuilder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);
            optionsBuilder.MigrationsHistoryTable("_MigrationHistory", schemaName);
        }));
        svc.AddHealthChecks().AddSqlServer(connectionString);
        svc.BuildServiceProvider().GetRequiredService<PersistenceContext>().Database.Migrate();
        return svc;
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = new Guid("DC12BB04-D629-4D8E-A023-5A8644F7F298"),
            FirstName = "Juan",
            LastName = "Ramirez",
            Email = "jr@correo.com"
        });
    }
}