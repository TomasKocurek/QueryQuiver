namespace QueryQuiver.Contracts;
public class DataList<T>
{
    public List<T> Data { get; init; } = [];
    public int PageSize { get; init; }
    public int Page { get; init; }
    public int TotalCount { get; init; }
}
