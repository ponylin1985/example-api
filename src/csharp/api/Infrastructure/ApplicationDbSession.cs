using Example.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;

namespace Example.Api.Infrastructure;

/// <summary>
/// Implementation of IDbSession using Entity Framework Core.
/// </summary>
public class ApplicationDbSession : IDbSession, IAsyncDisposable
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
    /// Gets an open database connection asynchronously.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<DbConnection> GetOpenConnectionAsync(CancellationToken ct = default)
    {
        var connection = _dbContext.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
        {
            await _dbContext.Database.OpenConnectionAsync(ct);
        }

        return connection;
    }

    /// <summary>
    /// Ensures that a database transaction is started.
    /// </summary>
    /// <param name="level"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<DbTransaction> EnsureTransactionAsync(
        IsolationLevel level = IsolationLevel.ReadCommitted,
        CancellationToken ct = default)
    {
        if (_currentTransaction is null)
        {
            _currentTransaction = await _dbContext.Database.BeginTransactionAsync(level, ct);
        }

        return _currentTransaction.GetDbTransaction();
    }

    /// <summary>
    /// Executes the specified action with a resilient execution strategy.
    /// </summary>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T> ExecuteResilientAsync<T>(Func<Task<T>> action)
    {
        if (_dbContext.Database.CurrentTransaction is not null)
        {
            return await action();
        }

        var strategy = _dbContext.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(action);
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
        if (_currentTransaction is null)
        {
            return;
        }

        try
        {
            await _currentTransaction.CommitAsync(ct);
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = default;
            await _dbContext.Database.CloseConnectionAsync();
        }
    }

    /// <summary>
    /// Rolls back the current database transaction.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        if (_currentTransaction is null)
        {
            return;
        }

        try
        {
            await _currentTransaction.RollbackAsync(ct);
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = default;
            await _dbContext.Database.CloseConnectionAsync();
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

    /// <summary>
    /// Asynchronously disposes the database session and its resources.
    /// </summary>
    /// <returns></returns>
    public async ValueTask DisposeAsync()
    {
        if (_currentTransaction is not null)
        {
            await _currentTransaction.DisposeAsync();
        }

        await _dbContext.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
