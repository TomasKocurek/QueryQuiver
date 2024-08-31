using QueryQuiver.Contracts;
using QueryQuiver.Query;

namespace QueryQuiver.Tests;

public class QueryParserTests
{
    [Fact]
    public void Parse_EmptyDictionary_ReturnsEmptyData()
    {
        //Arrange
        Dictionary<string, string[]> rawFilters = [];

        //Act
        var queryData = QueryParser.Parse<object, object>(rawFilters);

        //Assert
        QueryData expectedQueryData = new(0, 20, null, []);
        Assert.Equal(expectedQueryData, queryData);
    }

    [Fact]
    public void Parse_ValidDictionary_ReturnsCorrectData()
    {
        //Arrange
        Dictionary<string, string[]> rawFilters = new()
        {
            {"page", ["10"]},
            {"pageSize", ["30"]},
            {"sort", ["column:desc"]},
            {"column", ["eq:value"]}
        };

        //Act
        var queryData = QueryParser.Parse<object, object>(rawFilters);

        //Assert
        QueryData expectedQueryData = new(10, 30, new SortItem("column", true), [new("column", "value", FilterOperator.Equal)]);
        Assert.Equal(expectedQueryData, queryData);
    }

    [Fact]
    public void Parse_InvalidFilter_ThrowsArgumentException()
    {
        //Arrange
        Dictionary<string, string[]> rawFilters = new()
        {
            {"column", ["invalid:value"]}
        };

        //Act
        void Act() => QueryParser.Parse<object, object>(rawFilters);

        //Assert
        Assert.Throws<ArgumentException>(Act);
    }

    [Fact]
    public void Parse_MultipleFilters_ReturnsCorrectData()
    {
        //Arrange
        Dictionary<string, string[]> rawFilters = new()
        {
            {"column1", ["eq:value1"]},
            {"column2", ["ne:value2"]}
        };

        //Act
        var queryData = QueryParser.Parse<object, object>(rawFilters);

        //Assert
        QueryData expectedQueryData = new(0, 20, null,
        [
            new FilterCondition("column1", "value1", FilterOperator.Equal),
            new FilterCondition("column2", "value2", FilterOperator.NotEqual)
        ]);
        Assert.Equal(expectedQueryData, queryData);
    }

    [Fact]
    public void Parse_ValueWithSpaces_ReturnsCorrectData()
    {
        //Arrange
        Dictionary<string, string[]> rawFilters = new()
        {
            {"column", ["eq:value with spaces"]}
        };

        //Act
        var queryData = QueryParser.Parse<object, object>(rawFilters);

        //Assert
        QueryData expectedQueryData = new(0, 20, null, [new("column", "value with spaces", FilterOperator.Equal)]);
        Assert.Equal(expectedQueryData, queryData);
    }

    [Fact]
    public void Parse_NestedProperties_ReturnsCorrectData()
    {
        //Arrange
        Dictionary<string, string[]> rawFilters = new()
        {
            {"nested|property", ["eq:value"]}
        };

        //Act
        var queryData = QueryParser.Parse<object, object>(rawFilters);

        //Assert
        QueryData expectedQueryData = new(0, 20, null, [new("nested.property", "value", FilterOperator.Equal)]);
        Assert.Equal(expectedQueryData, queryData);
    }
}