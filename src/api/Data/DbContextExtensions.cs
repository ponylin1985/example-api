using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Example.Api.Data;

public static class DbContextExtensions
{
    public static IServiceCollection AddApplicationDbContext(
            this IServiceCollection services,
            IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("DefaultConnection connection string is not configured.");
        }

        var npgsqlBuilder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Pooling = true,
            MinPoolSize = 10,
            MaxPoolSize = 100,
        };

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(npgsqlBuilder.ToString(), npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorCodesToAdd: null);
            });
        });

        return services;
    }
}
