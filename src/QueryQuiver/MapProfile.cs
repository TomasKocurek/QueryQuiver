namespace QueryQuiver;
public abstract class MapProfile<TSource, TDestination>
{
    protected Dictionary<string, string> MapDictionary { get; } = [];

    protected void AddProperty(string source, string destination)
        => MapDictionary.Add(source, destination);

    public string Get(string source)
        => MapDictionary.TryGetValue(source, out var destination) ? destination : source;
}
