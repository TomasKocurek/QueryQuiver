using QueryQuiver.Tests.Extensions;
using QueryQuiver.Tests.Fixtures;
using QueryQuiver.Tests.Mocks;

namespace QueryQuiver.Tests;

[Collection(nameof(DbContextCollection))]
public class PaginationTests(DbContextFixture dbContextFixture)
{
    TestDbContext dbContext => dbContextFixture.DbContext;

    [Fact]
    public void Paginate_NoPagination_ReturnsDefault()
    {
        //Arrange
        Dictionary<string, string[]> rawFilters = [];

        //Act
        var result = dbContext.People.ApplyFilters(rawFilters).ToList();

        //Assert
        var expected = dbContext.People
            .ApplyPagination(0, 20)
            .ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Paginate_WithPagination()
    {
        //Arrange
        Dictionary<string, string[]> rawFilters = new()
        {
            { "page", ["1"] },
            { "pageSize", ["10"] }
        };

        //Act
        var result = dbContext.People.ApplyFilters(rawFilters).ToList();

        //Assert
        var expected = dbContext.People
            .ApplyPagination(1, 10)
            .ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Paginate_WithPaginationAndDescSort()
    {
        //Arrange
        Dictionary<string, string[]> rawFilters = new()
        {
            { "page", ["1"] },
            { "pageSize", ["10"] },
            { "sort", ["LastName:desc"] }
        };

        //Act
        var result = dbContext.People.ApplyFilters(rawFilters).ToList();

        //Assert
        var expected = dbContext.People
            .OrderByDescending(p => p.LastName)
            .ApplyPagination(1, 10)
            .ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Paginate_WithPaginationAndAscSort()
    {
        //Arrange
        Dictionary<string, string[]> rawFilters = new()
        {
            { "page", ["3"] },
            { "pageSize", ["30"] },
            { "sort", ["Age:asc"] }
        };

        //Act
        var result = dbContext.People.ApplyFilters(rawFilters).ToList();

        //Assert
        var expected = dbContext.People
            .OrderBy(p => p.Age)
            .ApplyPagination(3, 30)
            .ToList();
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }
}
