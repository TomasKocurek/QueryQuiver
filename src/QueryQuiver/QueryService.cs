using Microsoft.EntityFrameworkCore;
using QueryQuiver.Contracts;

namespace QueryQuiver;
public class QueryService<TDto, TEntity>(MapProfile<TDto, TEntity> MapProfile)
{
    public async Task<DataList<TEntity>> ExecuteAsync(IQueryable<TEntity> query, IDictionary<string, string[]> rawFilters)
    {
        var queryData = QueryParser.Parse(rawFilters, MapProfile);
        var filters = QueryBuilder.BuildQuery<TEntity>(queryData.Filters);

        var queryWithFilters = query.Where(filters);
        var totalCount = await queryWithFilters.CountAsync();

        var result = await queryWithFilters
            .ApplySort(queryData.SortItem)
            .ApplyPagination(queryData)
            .ToListAsync();

        return new()
        {
            Page = queryData.Page,
            PageSize = queryData.PageSize,
            TotalCount = totalCount,
            Data = result
        };
    }

    public DataList<TEntity> Execute(IQueryable<TEntity> query, IDictionary<string, string[]> rawFilters)
    {
        var queryData = QueryParser.Parse(rawFilters, MapProfile);
        var filters = QueryBuilder.BuildQuery<TEntity>(queryData.Filters);

        var queryWithFilters = query.Where(filters);
        var totalCount = queryWithFilters.Count();

        var result = queryWithFilters
            .ApplySort(queryData.SortItem)
            .ApplyPagination(queryData)
            .ToList();

        return new()
        {
            Page = queryData.Page,
            PageSize = queryData.PageSize,
            TotalCount = totalCount,
            Data = result
        };
    }
}
