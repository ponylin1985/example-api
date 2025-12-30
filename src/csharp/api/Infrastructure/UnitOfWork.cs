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
    public async Task<IUnitOfWorkTransaction> BeginTransactionAsync(IsolationLevel level = IsolationLevel.ReadCommitted, CancellationToken ct = default)
    {
        await _dbSession.EnsureTransactionAsync(level, ct);
        return new UnitOfWorkTransactionWrapper(this);
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

    /// <summary>
    /// A wrapper for the database transaction that supports both synchronous and asynchronous disposal.
    /// </summary>
    private class UnitOfWorkTransactionWrapper : IUnitOfWorkTransaction
    {
        /// <summary>
        /// The underlying Unit of Work instance.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkTransactionWrapper"/> class.
        /// </summary>
        /// <param name="unitOfWork">The underlying Unit of Work instance.</param>
        public UnitOfWorkTransactionWrapper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Rollback the transaction upon disposal.
        /// </summary>
        public void Dispose()
        {
            _unitOfWork.RollbackTransactionAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Rollback the transaction upon asynchronous disposal.
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            await _unitOfWork.RollbackTransactionAsync();
        }
    }
}
