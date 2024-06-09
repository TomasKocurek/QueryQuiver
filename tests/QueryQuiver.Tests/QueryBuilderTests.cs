using QueryQuiver.Contracts;
using QueryQuiver.Tests.Fixtures;
using QueryQuiver.Tests.Mocks;
using QueryQuiver.Tests.Models;

namespace QueryQuiver.Tests;
[Collection(nameof(DbContextCollection))]
public class QueryBuilderTests(DbContextFixture dbContextFixture)
{
    TestDbContext dbContext => dbContextFixture.DbContext;

    [Fact]
    public void Filter_NoFilter_ReturnsAll()
    {
        //Arrange
        List<FilterCondition> filters = [];

        //Act
        var query = QueryBuilder.BuildQuery<PersonEntity>(filters);
        var result = dbContext.People.Where(query).ToList();

        //Assert
        var expected = dbContext.People.ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Filter_Equal()
    {
        //Arrange
        var name = dbContext.People.First().FirstName;
        List<FilterCondition> filters =
        [
            new(nameof(PersonEntity.FirstName), name, FilterOperator.Equal)
        ];

        //Act
        var query = QueryBuilder.BuildQuery<PersonEntity>(filters);
        var result = dbContext.People.Where(query).ToList();

        //Assert
        var expected = dbContext.People.Where(p => p.FirstName == name).ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Filter_GreaterThan()
    {
        //Arrange
        var age = dbContext.People.First().Age;
        List<FilterCondition> filters =
        [
            new(nameof(PersonEntity.Age), age.ToString(), FilterOperator.GreaterThan)
        ];

        //Act
        var query = QueryBuilder.BuildQuery<PersonEntity>(filters);
        var result = dbContext.People.Where(query).ToList();

        //Assert
        var expected = dbContext.People.Where(p => p.Age > age).ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Filter_StringContains()
    {
        //Act
        var filterValue = dbContext.People.First().LastName.Substring(1, 2);
        List<FilterCondition> filters =
        [
            new(nameof(PersonEntity.LastName), filterValue, FilterOperator.Contains)
        ];

        //Act
        var query = QueryBuilder.BuildQuery<PersonEntity>(filters);
        var result = dbContext.People.Where(query).ToList();

        //Assert
        var expected = dbContext.People.Where(p => p.LastName.Contains(filterValue)).ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Filter_CompositeFilter()
    {
        //Act
        var person = dbContext.People.First();
        var name = person.FirstName[..2];
        var age = person.Age;
        List<FilterCondition> filters =
        [
            new(nameof(PersonEntity.FirstName), name, FilterOperator.Contains),
            new(nameof(PersonEntity.Age), age.ToString(), FilterOperator.GreaterThanOrEqual)
        ];

        //Act
        var query = QueryBuilder.BuildQuery<PersonEntity>(filters);
        var result = dbContext.People.Where(query).ToList();

        //Assert
        var expected = dbContext.People.Where(p => p.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) && p.Age >= age).ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Filter_NumericRange()
    {
        //Act
        var minAge = dbContext.People.First().Age;
        var maxAge = minAge + 20;
        List<FilterCondition> filters =
        [
            new(nameof(PersonEntity.Age), minAge.ToString(), FilterOperator.GreaterThanOrEqual),
            new(nameof(PersonEntity.Age), maxAge.ToString(), FilterOperator.LessThanOrEqual)
        ];

        //Act
        var query = QueryBuilder.BuildQuery<PersonEntity>(filters);
        var result = dbContext.People.Where(query).ToList();

        //Assert
        var expected = dbContext.People.Where(p => p.Age >= minAge && p.Age <= maxAge).ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Filter_CaseInsensitive()
    {
        //Act
        var name = dbContext.People.First().FirstName;
        List<FilterCondition> filters =
        [
            new(nameof(PersonEntity.FirstName), name.ToUpper(), FilterOperator.Equal)
        ];

        //Act
        var query = QueryBuilder.BuildQuery<PersonEntity>(filters);
        var result = dbContext.People.Where(query).ToList();

        //Assert
        var expected = dbContext.People.Where(p => p.FirstName == name).ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Filter_NonExistentProperty_ThrowsArgumentException()
    {
        //Arrange
        List<FilterCondition> filters =
        [
            new("NonExistentProperty", "value", FilterOperator.Equal)
        ];

        //Act
        void Act() => QueryBuilder.BuildQuery<PersonEntity>(filters);

        //Assert
        Assert.Throws<ArgumentException>(Act);
    }

    [Fact]
    public void Filter_BooleanProperty()
    {
        //Arrange
        List<FilterCondition> filters = [
            new(nameof(PersonEntity.IsEmployed), "true", FilterOperator.Equal)
        ];

        //Act
        var query = QueryBuilder.BuildQuery<PersonEntity>(filters);
        var result = dbContext.People.Where(query).ToList();

        //Assert
        var expected = dbContext.People.Where(p => p.IsEmployed).ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }
}
