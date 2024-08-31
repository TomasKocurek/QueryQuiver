using System.Linq.Expressions;

namespace QueryQuiver.Mapping;
/// <summary>
/// Abstract class, for creating mapping for properties
/// </summary>
/// <typeparam name="TDto">Dto of entity</typeparam>
/// <typeparam name="TEntity">Database entity</typeparam>
public abstract class MappingProfile<TDto, TEntity>
{
    protected Dictionary<string, string> MapDictionary { get; } = new(StringComparer.OrdinalIgnoreCase);

    private void AddProperty(string source, string destination)
        => MapDictionary.Add(source, destination);

    public string Get(string source)
        => MapDictionary.TryGetValue(source, out var destination) ? destination : source;

    /// <summary>
    /// Set mapping between properties 
    /// </summary>
    /// <typeparam name="TDtoProperty"></typeparam>
    /// <typeparam name="TEntityProperty"></typeparam>
    /// <param name="dtoProperty"></param>
    /// <param name="entityProperty"></param>
    public void MapProperty<TDtoProperty, TEntityProperty>(Expression<Func<TDto, TDtoProperty>> dtoProperty, Expression<Func<TEntity, TEntityProperty>> entityProperty)
    {
        var dtoPropertyPath = GetFullPropertyPath(dtoProperty);
        var entityPropertyPath = GetFullPropertyPath(entityProperty);

        AddProperty(dtoPropertyPath, entityPropertyPath);
    }

    private static string GetFullPropertyPath<T, TProperty>(Expression<Func<T, TProperty>> selector)
    {
        var path = new List<string>();
        Expression expression = selector.Body;

        while (expression is MemberExpression memberExpression && memberExpression.Expression is not null)
        {
            path.Insert(0, memberExpression.Member.Name);
            expression = memberExpression.Expression;
        }

        if (path.Count == 0)
        {
            throw new ArgumentException("Selector must be a member expression");
        }

        return string.Join(".", path);
    }
}
