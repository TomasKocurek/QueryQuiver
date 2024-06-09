using QueryQuiver.Contracts;

namespace QueryQuiver;
public static class QueryableExtensions
{
    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, IDictionary<string, string[]> rawFilters)
    {
        var queryData = QueryParser.Parse(rawFilters);
        var filters = QueryBuilder.BuildQuery<T>(queryData.Filters);

        return query
            .Where(filters)
            .ApplySort(queryData.SortItem)
            .ApplyPagination(queryData)
            .Take(queryData.PageSize);
    }

    private static IQueryable<T> ApplySort<T>(this IQueryable<T> query, SortItem? sortItem)
    {
        if (sortItem is null)
            return query;

        var sortProperty = SortBuilder.CreateSortProperty<T>(sortItem);

        if (sortProperty is null)
            return query;

        return sortItem.Descending
            ? query.OrderByDescending(sortProperty)
            : query.OrderBy(sortProperty);
    }

    private static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, QueryData queryData)
        => query.Skip(queryData.Page * queryData.PageSize).Take(queryData.PageSize);
}
