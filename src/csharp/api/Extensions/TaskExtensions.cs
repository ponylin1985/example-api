namespace Example.Api.Extensions;

/// <summary>
/// Extension methods for Task.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Executes an action after the task completes and returns the original result.
    /// </summary>
    /// <param name="task"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<T> Then<T>(this Task<T> task, Action<T> action)
    {
        var result = await task;
        action(result);
        return result;
    }

    /// <summary>
    /// Executes an asynchronous function after the task completes and returns the result of the function.
    /// </summary>
    /// <param name="task"></param>
    /// <param name="func"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static async Task<TResult> ThenAsync<T, TResult>(this Task<T> task, Func<T, Task<TResult>> func)
    {
        var result = await task;
        return await func(result);
    }
}
