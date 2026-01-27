using System.Data;

namespace Example.Api.Infrastructure;

/// <summary>
/// Unit of Work implementation for managing database transactions.
/// </summary>
public class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    /// <summary>
    /// The database session.
    /// </summary>
    private readonly IDbSession _dbSession;

    /// <summary>
    /// The active transaction wrapper.
    /// </summary>
    private UnitOfWorkTransactionWrapper? _activeWrapper;

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
    /// <param name="level">The isolation level of the transaction.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the transaction wrapper.</returns>
    public async Task<IUnitOfWorkTransaction> BeginTransactionAsync(
        IsolationLevel level = IsolationLevel.ReadCommitted, CancellationToken ct = default)
    {
        if (_activeWrapper is not null)
        {
            throw new InvalidOperationException("A transaction is already in progress for this unit of work.");
        }

        await _dbSession.EnsureTransactionAsync(level, ct);
        _activeWrapper = new UnitOfWorkTransactionWrapper(this, () => _activeWrapper = null);
        return _activeWrapper;
    }

    /// <summary>
    /// Commits all changes tracked by EF Core and commits the underlying database transaction.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSession.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Commits the underlying database transaction.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dbSession.CommitTransactionAsync(cancellationToken);
        _activeWrapper?.Complete();
    }

    /// <summary>
    /// Rolls back the underlying database transaction.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dbSession.RollbackTransactionAsync(cancellationToken);
        _activeWrapper?.Complete();
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
    /// Asynchronously disposes the Unit of Work and its resources.
    /// </summary>
    /// <returns></returns>
    public async ValueTask DisposeAsync()
    {
        if (_dbSession is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync();
        }
        else
        {
            _dbSession.Dispose();
        }
        
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
        /// Indicates whether the transaction has been completed or disposed.
        /// </summary>
        private bool _isCompleted;

        /// <summary>
        /// Indicates whether the transaction has been disposed.
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        /// An callback to invoke upon disposal.
        /// </summary>
        private readonly Action? _onDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkTransactionWrapper"/> class.
        /// </summary>
        /// <param name="unitOfWork">The underlying Unit of Work instance.</param>
        /// <param name="onDisposed">An optional callback to invoke upon disposal.</param>
        public UnitOfWorkTransactionWrapper(IUnitOfWork unitOfWork, Action onDisposed)
        {
            _unitOfWork = unitOfWork;
            _onDisposed = onDisposed;
        }

        /// <summary>
        /// Marks the transaction as complete, preventing rollback on disposal.
        /// </summary>
        public void Complete() => _isCompleted = true;

        /// <summary>
        /// Cleans up resources upon disposal.
        /// </summary>
        private void Cleanup()
        {
            if (_isDisposed) return;
            _isDisposed = true;
            _onDisposed?.Invoke();
        }

        /// <summary>
        /// Rollback the transaction upon disposal.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed || _isCompleted)
            {
                Cleanup();
                return;
            }
            

            try
            {
                _unitOfWork.RollbackTransactionAsync()
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            finally
            {
                _isDisposed = true;
                Cleanup();
            }
        }

        /// <summary>
        /// Rollback the transaction upon asynchronous disposal.
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            if (_isDisposed || _isCompleted)
            {
                Cleanup();
                return;
            }

            try
            {
                await _unitOfWork.RollbackTransactionAsync();
            }
            finally
            {
                _isDisposed = true;
                Cleanup();
            }
        }
    }
}
