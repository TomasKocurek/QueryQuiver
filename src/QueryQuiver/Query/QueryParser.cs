using QueryQuiver.Contracts;
using QueryQuiver.Helpers;
using QueryQuiver.Mapping;

namespace QueryQuiver.Query;
public static class QueryParser
{
    public static QueryData Parse<TDto, TEntity>(IDictionary<string, string[]> rawFilters, MappingProfile<TEntity, TDto>? mapProfile = null)
    {
        var page = GetIntValue(rawFilters, Constants.PageKey, Constants.DefaultPage);
        var pageSize = GetIntValue(rawFilters, Constants.PageSizeKey, Constants.DefaultPageSize);
        var sort = GetSortItem(rawFilters, mapProfile);
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

    private static SortItem? GetSortItem<TDto, TEntity>(IDictionary<string, string[]> filters, MappingProfile<TEntity, TDto>? mapProfile)
    {
        if (!filters.TryGetValue(Constants.SortKey, out var value))
            return null;

        var (originalColumn, directionValue) = SplitByFirstSeparator(value[0]);

        var column = mapProfile?.Get(originalColumn) ?? originalColumn;
        bool descending = directionValue == Constants.Descending;
        filters.Remove(Constants.SortKey);

        return new(column, descending);
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
        var column = unparsedColumn.Replace(Constants.NestedSeparator, ".");
        var (unparsedOperator, value) = SplitByFirstSeparator(unparsedFilter);

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

    private static (string First, string Second) SplitByFirstSeparator(string originalString)
    {
        var separatorIndex = originalString.IndexOf(Constants.ValueSeparator);
        if (separatorIndex == -1)
            throw new ArgumentException($"Invalid format {originalString}");

        var first = originalString[..separatorIndex];
        var second = originalString[(separatorIndex + 1)..];

        return (first, second);
    }
}
