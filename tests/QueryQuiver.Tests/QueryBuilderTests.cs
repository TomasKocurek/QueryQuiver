using Microsoft.EntityFrameworkCore;
using QueryQuiver.Contracts;
using QueryQuiver.Tests.Fixtures;
using QueryQuiver.Tests.Mocks;
using QueryQuiver.Tests.Models.Entities;

namespace QueryQuiver.Tests;
[Collection(nameof(QueryQuiverCollection))]
public class QueryBuilderTests(DbContextFixture dbContextFixture)
{
    private readonly TestDbContext _dbContext = dbContextFixture.DbContext;

    [Fact]
    public void Filter_NoFilter_ReturnsAll()
    {
        //Arrange
        List<FilterCondition> filters = [];

        //Act
        var query = QueryBuilder.BuildQuery<PersonEntity>(filters);
        var result = _dbContext.People.Where(query).ToList();

        //Assert
        var expected = _dbContext.People.ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Filter_Equal()
    {
        //Arrange
        var name = _dbContext.People.First().FirstName;
        List<FilterCondition> filters =
        [
            new(nameof(PersonEntity.FirstName), name, FilterOperator.Equal)
        ];

        //Act
        var query = QueryBuilder.BuildQuery<PersonEntity>(filters);
        var result = _dbContext.People.Where(query).ToList();

        //Assert
        var expected = _dbContext.People.Where(p => p.FirstName == name).ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Filter_GreaterThan()
    {
        //Arrange
        var age = _dbContext.People.First().Age;
        List<FilterCondition> filters =
        [
            new(nameof(PersonEntity.Age), age.ToString(), FilterOperator.GreaterThan)
        ];

        //Act
        var query = QueryBuilder.BuildQuery<PersonEntity>(filters);
        var result = _dbContext.People.Where(query).ToList();

        //Assert
        var expected = _dbContext.People.Where(p => p.Age > age).ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Filter_StringContains()
    {
        //Act
        var filterValue = _dbContext.People.First().LastName.Substring(1, 2);
        List<FilterCondition> filters =
        [
            new(nameof(PersonEntity.LastName), filterValue, FilterOperator.Contains)
        ];

        //Act
        var query = QueryBuilder.BuildQuery<PersonEntity>(filters);
        var result = _dbContext.People.Where(query).ToList();

        //Assert
        var expected = _dbContext.People.Where(p => p.LastName.Contains(filterValue)).ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Filter_CompositeFilter()
    {
        //Act
        var person = _dbContext.People.First();
        var name = person.FirstName[..2];
        var age = person.Age;
        List<FilterCondition> filters =
        [
            new(nameof(PersonEntity.FirstName), name, FilterOperator.Contains),
            new(nameof(PersonEntity.Age), age.ToString(), FilterOperator.GreaterThanOrEqual)
        ];

        //Act
        var query = QueryBuilder.BuildQuery<PersonEntity>(filters);
        var result = _dbContext.People.Where(query).ToList();

        //Assert
        var expected = _dbContext.People.Where(p => p.FirstName.Contains(name) && p.Age >= age).ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Filter_NumericRange()
    {
        //Act
        var minAge = _dbContext.People.First().Age;
        var maxAge = minAge + 20;
        List<FilterCondition> filters =
        [
            new(nameof(PersonEntity.Age), minAge.ToString(), FilterOperator.GreaterThanOrEqual),
            new(nameof(PersonEntity.Age), maxAge.ToString(), FilterOperator.LessThanOrEqual)
        ];

        //Act
        var query = QueryBuilder.BuildQuery<PersonEntity>(filters);
        var result = _dbContext.People.Where(query).ToList();

        //Assert
        var expected = _dbContext.People.Where(p => p.Age >= minAge && p.Age <= maxAge).ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Filter_CaseInsensitive()
    {
        //Act
        var name = _dbContext.People.First().FirstName;
        List<FilterCondition> filters =
        [
            new(nameof(PersonEntity.FirstName), name.ToUpper(), FilterOperator.Equal)
        ];

        //Act
        var query = QueryBuilder.BuildQuery<PersonEntity>(filters);
        var result = _dbContext.People.Where(query).ToList();

        //Assert
        var expected = _dbContext.People.Where(p => p.FirstName == name).ToList();
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
            new(nameof(PersonEntity.GDPR), "true", FilterOperator.Equal)
        ];

        //Act
        var query = QueryBuilder.BuildQuery<PersonEntity>(filters);
        var result = _dbContext.People.Where(query).ToList();

        //Assert
        var expected = _dbContext.People.Where(p => p.GDPR).ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Filter_OwnedProperty()
    {
        //Arrange
        var address = _dbContext.People.First().Address;
        List<FilterCondition> filters = [
            new($"{nameof(PersonEntity.Address)}.{nameof(Address.Country)}", address.Country, FilterOperator.Equal)
        ];

        //Act
        var query = QueryBuilder.BuildQuery<PersonEntity>(filters);
        var result = _dbContext.People.Where(query).ToList();

        //Assert
        var expected = _dbContext.People.Where(p => p.Address.Country == address.Country).ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Filter_NestedProperty()
    {
        //Arrange
        var job = _dbContext.People
            .Include(p => p.Job)
            .First().Job;
        List<FilterCondition> filters = [
            new($"{nameof(PersonEntity.Job)}.{nameof(JobEntity.Title)}", job.Title, FilterOperator.Equal)
        ];

        //Act
        var query = QueryBuilder.BuildQuery<PersonEntity>(filters);
        var result = _dbContext.People.Where(query).ToList();

        //Assert
        var expected = _dbContext.People.Where(p => p.Job.Title == job.Title).ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }
}
