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

        svc.AddDbContext<PersistenceContext>(_ => _.UseSqlServer(connectionString, _ =>
        {
            _.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);
            _.MigrationsHistoryTable("_MigrationHistory", schemaName);
        }));
        svc.AddHealthChecks().AddSqlServer(connectionString);
        return svc;
    }
}