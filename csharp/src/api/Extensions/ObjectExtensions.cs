using System.Collections;

namespace Example.Api.Extensions;

/// <summary>
/// Extension methods for object types.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Determines whether the specified object is null or an empty collection.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this object source)
    {
        if (source is null)
        {
            return true;
        }

        if (source is IEnumerable enumerable)
        {
            if (source is ICollection collection)
            {
                return collection.Count == 0;
            }

            if (source.GetType().IsGenericType &&
                source.GetType().GetGenericTypeDefinition() == typeof(IQueryable<>))
            {
                return !((IQueryable)source).Cast<object>().Any();
            }

            return !enumerable.Cast<object>().Any();
        }

        return false;
    }
}

