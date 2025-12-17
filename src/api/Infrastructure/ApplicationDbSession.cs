using Example.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;

namespace Example.Api.Infrastructure;

/// <summary>
/// Implementation of IDbSession using Entity Framework Core.
/// </summary>
public class ApplicationDbSession : IDbSession
{
    /// <summary>
    /// The Entity Framework Core DbContext instance for the current session scope.
    /// </summary>
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// The current database transaction.
    /// </summary>
    private IDbContextTransaction? _currentTransaction;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbSession"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    public ApplicationDbSession(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Gets the Entity Framework Core DbContext instance for the current session scope.
    /// </summary>
    public DbContext DataContext => _dbContext;

    /// <summary>
    /// Gets the current database connection.
    /// </summary>
    /// <returns></returns>
    public DbConnection? CurrentConnection => _dbContext.Database.GetDbConnection();

    /// <summary>
    /// Gets the current database transaction.
    /// </summary>
    /// <returns></returns>
    public DbTransaction? CurrentTransaction => _currentTransaction?.GetDbTransaction();

    /// <summary>
    /// Gets an open database connection asynchronously.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<DbConnection> GetOpenConnectionAsync(CancellationToken ct = default)
    {
        var connection = _dbContext.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(ct);
        }

        return connection;
    }

    /// <summary>
    /// Ensures that a database transaction is started.
    /// </summary>
    /// <param name="level"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task EnsureTransactionAsync(IsolationLevel level = IsolationLevel.ReadCommitted, CancellationToken ct = default)
    {
        if (_currentTransaction is not null)
        {
            return;
        }

        _currentTransaction = await _dbContext.Database.BeginTransactionAsync(level, ct);
    }

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _dbContext.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Commits the current database transaction.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        if (_currentTransaction is not null)
        {
            await _currentTransaction.CommitAsync(ct);
        }
    }

    /// <summary>
    /// Rolls back the current database transaction.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync(ct);
        }
    }

    /// <summary>
    /// Disposes the database session and its resources.
    /// </summary>
    public void Dispose()
    {
        _currentTransaction?.Dispose();
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}
