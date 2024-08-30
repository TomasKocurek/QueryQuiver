namespace QueryQuiver;
/// <summary>
/// Abstract class, for creating mapping for properties
/// </summary>
/// <typeparam name="TDto">Dto of entity</typeparam>
/// <typeparam name="TEntity">Database entity</typeparam>
public abstract class MapProfile<TDto, TEntity>
{
    protected Dictionary<string, string> MapDictionary { get; } = new(StringComparer.OrdinalIgnoreCase);

    protected void AddProperty(string source, string destination)
        => MapDictionary.Add(source, destination);

    public string Get(string source)
        => MapDictionary.TryGetValue(source, out var destination) ? destination : source;
}
