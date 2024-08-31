using Microsoft.Extensions.DependencyInjection;
using QueryQuiver.Interfaces;
using QueryQuiver.Tests.Extensions;
using QueryQuiver.Tests.Fixtures;
using QueryQuiver.Tests.Mocks;
using QueryQuiver.Tests.Models.Dtos;
using QueryQuiver.Tests.Models.Entities;

namespace QueryQuiver.Tests;

[Collection(nameof(QueryQuiverCollection))]
public class PaginationTests(DbContextFixture dbContextFixture, ServiceProviderFixture serviceProviderFixture)
{
    private readonly TestDbContext _dbContext = dbContextFixture.DbContext;
    private readonly IFilteringService _queryService = serviceProviderFixture.ServiceProvider.GetRequiredService<IFilteringService>();


    [Fact]
    public async Task Paginate_NoPagination_ReturnsDefault()
    {
        //Arrange
        Dictionary<string, string[]> rawFilters = [];

        //Act
        var result = await _queryService.ExecuteAsync<PersonDto, PersonEntity>(_dbContext.People, rawFilters);

        //Assert
        var expected = _dbContext.People
            .ApplyPagination(0, 20)
            .ToList();
        Assert.Equal(expected, result.Data);
        Assert.NotEmpty(result.Data);
    }

    [Fact]
    public async Task Paginate_WithPagination()
    {
        //Arrange
        Dictionary<string, string[]> rawFilters = new()
        {
            { "page", ["1"] },
            { "pageSize", ["10"] }
        };

        //Act
        var result = await _queryService.ExecuteAsync<PersonDto, PersonEntity>(_dbContext.People, rawFilters);

        //Assert
        var expected = _dbContext.People
            .ApplyPagination(1, 10)
            .ToList();
        Assert.Equal(expected, result.Data);
        Assert.NotEmpty(result.Data);
    }

    [Fact]
    public async Task Paginate_WithPaginationAndDescSort()
    {
        //Arrange
        Dictionary<string, string[]> rawFilters = new()
        {
            { "page", ["1"] },
            { "pageSize", ["10"] },
            { "sort", ["LastName:desc"] }
        };

        //Act
        var result = await _queryService.ExecuteAsync<PersonDto, PersonEntity>(_dbContext.People, rawFilters);

        //Assert
        var expected = _dbContext.People
            .OrderByDescending(p => p.LastName)
            .ApplyPagination(1, 10)
            .ToList();
        Assert.Equal(expected, result.Data);
        Assert.NotEmpty(result.Data);
    }

    [Fact]
    public async Task Paginate_WithPaginationAndAscSort()
    {
        //Arrange
        Dictionary<string, string[]> rawFilters = new()
        {
            { "page", ["3"] },
            { "pageSize", ["30"] },
            { "sort", ["Age:asc"] }
        };

        //Act
        var result = await _queryService.ExecuteAsync<PersonDto, PersonEntity>(_dbContext.People, rawFilters);

        //Assert
        var expected = _dbContext.People
            .OrderBy(p => p.Age)
            .ApplyPagination(3, 30)
            .ToList();
        Assert.Equal(expected, result.Data);
        Assert.NotEmpty(result.Data);
    }
}
