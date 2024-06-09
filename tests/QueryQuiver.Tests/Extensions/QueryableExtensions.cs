namespace QueryQuiver.Tests.Extensions;
internal static class QueryableExtensions
{
    internal static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int page, int pageSize)
        => query.Skip(page * pageSize).Take(pageSize);
}
