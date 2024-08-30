using QueryQuiver.Contracts;

namespace QueryQuiver;
public static class QueryParser
{
    public static QueryData Parse<TDto, TEntity>(IDictionary<string, string[]> rawFilters, MappingProfile<TEntity, TDto>? mapProfile = null)
    {
        var page = GetIntValue(rawFilters, "page", 0);
        var pageSize = GetIntValue(rawFilters, "pageSize", 20);
        var sort = GetSortItem(rawFilters);
        var filterConditions = ParseFilters(rawFilters, mapProfile);

        return new QueryData(page, pageSize, sort, filterConditions);
    }

    private static int GetIntValue(IDictionary<string, string[]> filters, string key, int defaultValue)
    {
        int parsedValue = defaultValue;
        if (filters.TryGetValue(key, out var value) && !int.TryParse(value[0], out parsedValue))
            parsedValue = defaultValue;
        else filters.Remove(key);

        return parsedValue;
    }

    private static SortItem? GetSortItem(IDictionary<string, string[]> filters)
    {
        if (!filters.TryGetValue("sort", out var value))
            return null;

        var parts = value[0].Split(':');
        var column = parts[0];
        bool direction = parts.Length > 1 && parts[1] == "desc";
        filters.Remove("sort");

        return new(column, direction);
    }

    private static List<FilterCondition> ParseFilters<TDto, TEntity>(IDictionary<string, string[]> filters, MappingProfile<TEntity, TDto>? mapProfile)
    {
        var filterConditions = new List<FilterCondition>();
        foreach (var filter in filters)
        {
            var column = mapProfile?.Get(filter.Key) ?? filter.Key;
            foreach (var value in filter.Value)
            {
                var filterCondition = ParseFilter(column, value);
                filterConditions.Add(filterCondition);
            }
        }

        return filterConditions;
    }

    private static FilterCondition ParseFilter(string unparsedColumn, string unparsedFilter)
    {
        var column = unparsedColumn.Replace("|", ".");

        var separatorIndex = unparsedFilter.IndexOf(':');
        if (separatorIndex == -1)
            throw new ArgumentException($"Invalid filter format: {unparsedColumn}");

        var unparsedOperator = unparsedFilter[..separatorIndex];
        var value = unparsedFilter[(separatorIndex + 1)..];

        var filterOperator = unparsedOperator switch
        {
            "eq" => FilterOperator.Equal,
            "ne" => FilterOperator.NotEqual,
            "gt" => FilterOperator.GreaterThan,
            "ge" => FilterOperator.GreaterThanOrEqual,
            "lt" => FilterOperator.LessThan,
            "le" => FilterOperator.LessThanOrEqual,
            "ct" => FilterOperator.Contains,
            "sw" => FilterOperator.StartsWith,
            "ew" => FilterOperator.EndsWith,
            _ => throw new ArgumentException($"Invalid filter operator: {value}")
        };

        return new FilterCondition(column, value, filterOperator);
    }
}
