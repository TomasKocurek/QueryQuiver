namespace QueryQuiver.Contracts;

public record FilterCondition(string PropertyName, string Value, FilterOperator Operator);