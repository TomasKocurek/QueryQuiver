using QueryQuiver.Contracts;
using System.Linq.Expressions;

namespace QueryQuiver.Query;

public static class QueryBuilder
{
    public static Expression<Func<T, bool>> BuildQuery<T>(IEnumerable<FilterCondition> filters)
    {
        if (!filters.Any())
            return _ => true;

        var parameter = Expression.Parameter(typeof(T), nameof(T));
        Expression? filterExpression = null;
        foreach (var filter in filters)
        {
            var property = CreateProperty(parameter, filter);
            var constant = CreateValueExpression(filter, property);
            var comparison = CreateComparisonExpression(property, constant, filter.Operator);

            filterExpression = filterExpression == null
                ? comparison
                : Expression.And(filterExpression, comparison);
        }

        return Expression.Lambda<Func<T, bool>>(filterExpression!, parameter);
    }

    private static ConstantExpression CreateValueExpression(FilterCondition filter, Expression property)
    {
        if (property.Type == typeof(string))
            return Expression.Constant(filter.Value.ToLower());
        else if (property.Type == typeof(decimal))
            return Expression.Constant(decimal.Parse(filter.Value));
        else if (property.Type == typeof(int))
            return Expression.Constant(int.Parse(filter.Value));
        else if (property.Type == typeof(long))
            return Expression.Constant(long.Parse(filter.Value));
        else if (property.Type == typeof(float))
            return Expression.Constant(float.Parse(filter.Value));
        else if (property.Type == typeof(bool))
            return Expression.Constant(bool.Parse(filter.Value));
        else if (property.Type.IsEnum)
            return Expression.Constant(Enum.Parse(property.Type, filter.Value));
        else
            throw new NotImplementedException($"Type {property.Type} is not supported for filtering");
    }

    private static Expression CreateComparisonExpression(Expression property, ConstantExpression constant, FilterOperator filterOperator)
        => filterOperator switch
        {
            FilterOperator.Equal => Expression.Equal(property, constant),
            FilterOperator.NotEqual => Expression.NotEqual(property, constant),
            FilterOperator.GreaterThan => Expression.GreaterThan(property, constant),
            FilterOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(property, constant),
            FilterOperator.LessThan => Expression.LessThan(property, constant),
            FilterOperator.LessThanOrEqual => Expression.LessThanOrEqual(property, constant),
            FilterOperator.Contains => Expression.Call(property, "Contains", null, constant),
            FilterOperator.StartsWith => Expression.Call(property, "StartsWith", null, constant),
            FilterOperator.EndsWith => Expression.Call(property, "EndsWith", null, constant),
            _ => throw new NotImplementedException($"Filter operator {filterOperator} is not supported")
        };

    private static Expression CreateProperty(ParameterExpression parameter, FilterCondition filter)
    {
        // Split the property name to support nested properties
        Expression property = CreatePathToProperty();
        property = ConvertToNonNullable();
        property = ConvertToLowerCase();
        return property;

        Expression ConvertToLowerCase()
        {
            if (property.Type == typeof(string))
            {
                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes)!;
                property = Expression.Call(property, toLowerMethod);
            }

            return property;
        }

        Expression ConvertToNonNullable()
        {
            var propertyType = Nullable.GetUnderlyingType(property.Type) ?? property.Type;
            return Expression.Convert(property, propertyType);
        }

        Expression CreatePathToProperty()
        {
            string[] parts = filter.PropertyName.Split('.');
            Expression property = parameter;
            foreach (var part in parts)
            {
                property = Expression.Property(property, part);
            }

            return property;
        }
    }
}