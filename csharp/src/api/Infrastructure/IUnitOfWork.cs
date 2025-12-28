using System.Data;

namespace Example.Api.Infrastructure;

/// <summary>
/// Unit of Work interface for managing database transactions.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Begins a new database transaction.
    /// </summary>
    /// <param name="level"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task BeginTransactionAsync(IsolationLevel level = IsolationLevel.ReadCommitted, CancellationToken ct = default);

    /// <summary>
    /// Saves all changes tracked by EF Core to the database.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the underlying database transaction.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current database transaction.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
