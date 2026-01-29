using System.Data;

namespace Example.Api.Infrastructure;

/// <summary>
/// Unit of Work interface for managing database transactions.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Executes the specified database operation with a EF Core built-in resilient execution strategy.
    /// </summary>
    /// <param name="action">A database operation action.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous execution operation.</returns>
    Task ExecuteStrategyAsync(
        Func<Task> action,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Begin a new database transaction.
    /// </summary>
    /// <param name="level">The isolation level for the transaction.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>Returns a wrapper for the database transaction.</returns>
    Task<IUnitOfWorkTransaction> BeginTransactionAsync(
        IsolationLevel level = IsolationLevel.ReadCommitted,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves all changes tracked by EF Core to the database.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>Returns the number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the underlying database transaction.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous commit operation.</returns>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current database transaction.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous rollback operation.</returns>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
