
namespace QueryQuiver.Contracts;

public class QueryData(int offset, int limit, SortItem? sortItem, IEnumerable<FilterCondition> filters)
{
    public int Offset { get; init; } = offset;
    public int Limit { get; init; } = limit;
    public SortItem? SortItem { get; init; } = sortItem;
    public IEnumerable<FilterCondition> Filters { get; init; } = filters;

    public override bool Equals(object? obj)
    {
        if (obj is not QueryData other)
            return false;

        return Offset == other.Offset
            && Limit == other.Limit
            && SortItem == other.SortItem
            && Filters.SequenceEqual(other.Filters);
    }

    public override int GetHashCode()
        => HashCode.Combine(Offset, Limit, SortItem, Filters);
}
