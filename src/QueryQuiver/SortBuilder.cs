using QueryQuiver.Contracts;
using System.Linq.Expressions;

namespace QueryQuiver;
public static class SortBuilder
{
    public static Expression<Func<T, bool>> CreateProperty<T>(SortItem? sortItem)
    {
        if (sortItem is null)
            return _ => true;

        var parameter = Expression.Parameter(typeof(T), nameof(T));
        var property = Expression.Property(parameter, sortItem.PropertyName);
        return Expression.Lambda<Func<T, bool>>(property!, parameter);
    }
}
