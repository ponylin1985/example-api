using Example.Api.Data;
using Example.Api.DateTimeOffsetProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

/// <summary>
/// A factory for creating derived <see cref="ApplicationDbContext"/> instances.
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    /// <summary>
    /// Creates a new instance of <see cref="ApplicationDbContext"/>.
    /// </summary>
    /// <param name="args">Arguments provided by the design-time tools.</param>
    /// <returns>An instance of <see cref="ApplicationDbContext"/>.</returns>
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
            .AddUserSecrets<Program>()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "DefaultConnection connection string not found in configuration. " +
                "Ensure it's set in appsettings.Development.json or via User Secrets.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseNpgsql(
            connectionString,
            options => options.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));

        return new ApplicationDbContext(optionsBuilder.Options, new DefaultDateTimeOffsetProvider());
    }
}