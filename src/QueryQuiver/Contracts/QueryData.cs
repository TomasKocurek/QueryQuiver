
namespace QueryQuiver.Contracts;

public class QueryData(int page, int pageSize, SortItem? sortItem, IEnumerable<FilterCondition> filters)
{
    public int Page { get; init; } = page;
    public int PageSize { get; init; } = pageSize;
    public SortItem? SortItem { get; init; } = sortItem;
    public IEnumerable<FilterCondition> Filters { get; init; } = filters;

    public override bool Equals(object? obj)
    {
        if (obj is not QueryData other)
            return false;

        return Page == other.Page
            && PageSize == other.PageSize
            && SortItem == other.SortItem
            && Filters.SequenceEqual(other.Filters);
    }

    public override int GetHashCode()
        => HashCode.Combine(Page, PageSize, SortItem, Filters);
}
