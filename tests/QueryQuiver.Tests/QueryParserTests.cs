using QueryQuiver.Contracts;

namespace QueryQuiver.Tests;

public class QueryParserTests
{
    [Fact]
    public void Parse_EmptyDictionary_ReturnsEmptyData()
    {
        //Arrange
        Dictionary<string, string[]> rawFilters = new();

        //Act
        var queryData = QueryParser.Parse(rawFilters);

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
            {"offset", new string[] {"10"}},
            {"limit", new string[] {"30"}},
            {"sort", new string[] {"column:desc"}},
            {"column", new string[] {"eq:value"}}
        };

        //Act
        var queryData = QueryParser.Parse(rawFilters);

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
            {"column", new string[] {"invalid:value"}}
        };

        //Act
        void Act() => QueryParser.Parse(rawFilters);

        //Assert
        Assert.Throws<ArgumentException>(Act);
    }

    [Fact]
    public void Parse_MultipleFilters_ReturnsCorrectData()
    {
        //Arrange
        Dictionary<string, string[]> rawFilters = new()
        {
            {"column1", new string[] {"eq:value1"}},
            {"column2", new string[] {"ne:value2"}}
        };

        //Act
        var queryData = QueryParser.Parse(rawFilters);

        //Assert
        QueryData expectedQueryData = new(0, 20, null,
        [
            new FilterCondition("column1", "value1", FilterOperator.Equal),
            new FilterCondition("column2", "value2", FilterOperator.NotEqual)
        ]);
        Assert.Equal(expectedQueryData, queryData);
    }
}