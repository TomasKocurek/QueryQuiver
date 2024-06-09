using QueryQuiver.Contracts;
using System.Linq.Expressions;

namespace QueryQuiver;
public static class SortBuilder
{
    public static Expression<Func<T, object>>? CreateSortProperty<T>(SortItem? sortItem)
    {
        if (sortItem is null)
            return null;

        var parameter = Expression.Parameter(typeof(T), nameof(T));
        var property = Expression.Property(parameter, sortItem.PropertyName);
        var convert = Expression.Convert(property, typeof(object));
        return Expression.Lambda<Func<T, object>>(convert, parameter);
    }
}
