using System.Data;

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
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="dbSession">The database session.</param>
    public UnitOfWork(IDbSession dbSession)
    {
        _dbSession = dbSession;
    }

    /// <summary>
    /// Begins a new database transaction.
    /// </summary>
    /// <param name="level"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public Task BeginTransactionAsync(IsolationLevel level = IsolationLevel.ReadCommitted, CancellationToken ct = default)
    {
        return _dbSession.EnsureTransactionAsync(level, ct);
    }

    /// <summary>
    /// Commits all changes tracked by EF Core and commits the underlying database transaction.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbSession.SaveChangesAsync(cancellationToken);
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _dbSession.CommitTransactionAsync(cancellationToken);
    }

    /// <summary>
    /// Rolls back the underlying database transaction.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _dbSession.RollbackTransactionAsync(cancellationToken);
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
