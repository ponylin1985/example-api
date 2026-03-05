using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Example.Api.Data;

public static class DbContextExtensions
{
    public static IServiceCollection AddApplicationDbContext(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment environment)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("DefaultConnection connection string is not configured.");
        }

        services.AddNpgsqlDataSource(connectionString);
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var dataSource = serviceProvider.GetRequiredService<NpgsqlDataSource>();

            options.UseNpgsql(dataSource, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 2,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: ["40001"]);
            });

            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
                options.LogTo(Console.WriteLine, LogLevel.Information);
            }
        });

        return services;
    }
}
