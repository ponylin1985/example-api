using Microsoft.EntityFrameworkCore;
using Npgsql;
using Polly;
using System.Data;
using System.Data.Common;

namespace Example.Api.Infrastructure;

/// <summary>
/// Unit of Work implementation for managing database transactions.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    /// <summary>
    /// The database session.
    /// </summary>
    private readonly IDbSession _dbSession;

    /// <summary>
    /// The retry policy for handling transient failures.
    /// </summary>
    private readonly AsyncPolicy _retryPolicy;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="dbSession">The database session.</param>
    public UnitOfWork(IDbSession dbSession)
    {
        _dbSession = dbSession;
        _retryPolicy = Policy
            .Handle<DbUpdateConcurrencyException>()
            .Or<DbUpdateException>()
            .Or<DbException>()
            .Or<NpgsqlException>(ex => ex.IsTransient)
            .Or<NpgsqlException>(ex => ex.SqlState == "40001")
            .Or<TimeoutException>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    /// <summary>
    /// Begins a new database transaction.
    /// </summary>
    /// <param name="level"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task BeginTransactionAsync(IsolationLevel level = IsolationLevel.ReadCommitted, CancellationToken ct = default)
    {
        await _dbSession.EnsureTransactionAsync(level, ct);
    }

    /// <summary>
    /// Commits all changes tracked by EF Core and commits the underlying database transaction.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _retryPolicy.ExecuteAsync(() =>
            _dbSession.SaveChangesAsync(cancellationToken));
    }

    /// <summary>
    /// Commits the underlying database transaction.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _retryPolicy.ExecuteAsync(() =>
            _dbSession.CommitTransactionAsync(cancellationToken));
    }

    /// <summary>
    /// Rolls back the underlying database transaction.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dbSession.RollbackTransactionAsync(cancellationToken);
    }

    /// <summary>
    /// Disposes the Unit of Work and its resources.
    /// </summary>
    public void Dispose()
    {
        _dbSession.Dispose();
        GC.SuppressFinalize(this);
    }
}
