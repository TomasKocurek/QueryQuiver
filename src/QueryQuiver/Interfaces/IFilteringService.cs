using QueryQuiver.Contracts;

namespace QueryQuiver.Interfaces;
/// <summary>
/// Service for filtering IQueryable data
/// </summary>
/// <typeparam name="TDto">Dto model</typeparam>
/// <typeparam name="TEntity">Database entity model</typeparam>
public interface IFilteringService<TDto, TEntity>
{
    Task<DataList<TEntity>> ExecuteAsync(IQueryable<TEntity> query, IDictionary<string, string[]> rawFilters);
    DataList<TEntity> Execute(IQueryable<TEntity> query, IDictionary<string, string[]> rawFilters);
}
