namespace QueryQuiver;
public static class QueryableExtensions
{
    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, IDictionary<string, string[]> rawFilters)
    {
        var queryData = QueryParser.Parse(rawFilters);
        var filters = QueryBuilder.BuildQuery<T>(queryData.Filters);
        var sort = SortBuilder.CreateProperty<T>(queryData.SortItem);

        return query
            .Where(filters)
            .OrderBy(sort)
            .Skip(queryData.Page * queryData.PageSize)
            .Take(queryData.PageSize);
    }
}
