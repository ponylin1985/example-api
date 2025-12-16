using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Example.Api.Infrastructure;

public interface IDbSession : IDisposable
{
    /// <summary>
    /// Gets the Entity Framework Core DbContext instance for the current session scope.
    /// This is used by Repositories for LINQ querying and entity tracking (CRUD).
    /// </summary>
    DbContext Context { get; }

    /// <summary>
    /// Gets the current database connection.
    /// </summary>
    DbConnection? CurrentConnection { get; }

    /// <summary>
    /// Gets the current database transaction.
    /// </summary>
    DbTransaction? CurrentTransaction { get; }

    /// <summary>
    /// Gets an open database connection.<para/>
    /// * This method is implemented to retrieve the connection managed by the underlying DbContext.<para/>
    /// </summary>
    Task<DbConnection> GetOpenConnectionAsync(CancellationToken ct = default);

    /// <summary>
    /// Ensures that a database transaction is started.<para/>
    /// * This operation utilizes the DbContext's built-in transaction manager.
    /// </summary>
    Task EnsureTransactionAsync(IsolationLevel level = IsolationLevel.ReadCommitted, CancellationToken ct = default);

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken ct = default);

    /// <summary>
    /// Commits the underlying database transaction.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task CommitTransactionAsync(CancellationToken ct = default);

    /// <summary>
    /// Rolls back the current database transaction.
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken ct = default);
}
