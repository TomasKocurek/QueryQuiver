using QueryQuiver.Contracts;

namespace QueryQuiver.Interfaces;
/// <summary>
/// Service for filtering IQueryable data
/// </summary>
public interface IFilteringService
{
    /// <summary>
    /// Filter and paginate data asynchronously
    /// </summary>
    /// <typeparam name="TDto">Dto Model</typeparam>
    /// <typeparam name="TEntity">Database entity model</typeparam>
    /// <param name="query">Data which on which filter should be applied</param>
    /// <param name="rawFilters">Raw query params which contains filters</param>
    /// <returns></returns>
    Task<DataList<TEntity>> ExecuteAsync<TDto, TEntity>(IQueryable<TEntity> query, IDictionary<string, string[]> rawFilters);
    /// <summary>
    /// Filter and paginate data
    /// </summary>
    /// <typeparam name="TDto">Dto Model</typeparam>
    /// <typeparam name="TEntity">Database entity model</typeparam>
    /// <param name="query">Data which on which filter should be applied</param>
    /// <param name="rawFilters">Raw query params which contains filters</param>
    /// <returns></returns>
    DataList<TEntity> Execute<TDto, TEntity>(IQueryable<TEntity> query, IDictionary<string, string[]> rawFilters);
}
