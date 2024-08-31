using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QueryQuiver.Contracts;
using QueryQuiver.Extensions;
using QueryQuiver.Interfaces;
using QueryQuiver.Mapping;

namespace QueryQuiver.Query;
internal class FilteringService(IServiceProvider ServiceProvider) : IFilteringService
{
    public DataList<TEntity> Execute<TDto, TEntity>(IQueryable<TEntity> query, IDictionary<string, string[]> rawFilters)
    {
        var mappingProfile = ServiceProvider.GetService<MappingProfile<TDto, TEntity>>();
        var queryData = QueryParser.Parse(rawFilters, mappingProfile);
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

    public async Task<DataList<TEntity>> ExecuteAsync<TDto, TEntity>(IQueryable<TEntity> query, IDictionary<string, string[]> rawFilters)
    {
        var mappingProfile = ServiceProvider.GetService<MappingProfile<TDto, TEntity>>();
        var queryData = QueryParser.Parse(rawFilters, mappingProfile);
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
}

