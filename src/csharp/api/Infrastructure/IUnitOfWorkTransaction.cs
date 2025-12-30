namespace Example.Api.Infrastructure;

/// <summary>
/// Represents a wrapper for a database transaction that supports both synchronous and asynchronous disposal.
/// </summary>
public interface IUnitOfWorkTransaction : IDisposable, IAsyncDisposable
{
}
