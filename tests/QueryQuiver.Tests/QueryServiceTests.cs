using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QueryQuiver.Interfaces;
using QueryQuiver.Tests.Fixtures;
using QueryQuiver.Tests.Mocks;
using QueryQuiver.Tests.Models.Dtos;
using QueryQuiver.Tests.Models.Entities;

namespace QueryQuiver.Tests;

[Collection(nameof(QueryQuiverCollection))]
public class QueryServiceTests(DbContextFixture DbContextFixture, ServiceProviderFixture serviceProviderFixture)
{
    private readonly TestDbContext _dbContext = DbContextFixture.DbContext;
    private readonly IFilteringService _queryService = serviceProviderFixture.ServiceProvider.GetRequiredService<IFilteringService>();

    [Fact]
    public async Task ExecuteQueryAsync_WithFilters_ReturnsFilteredData()
    {
        //Arrange
        var name = (await _dbContext.People.FirstAsync()).FirstName;
        Dictionary<string, string[]> rawFilters = new()
        {
            {"page", ["0"] },
            {"pageSize", ["10"] },
            {"firstName", [$"eq:{name}"]}
        };

        // Act
        var result = await _queryService.ExecuteAsync<PersonDto, PersonEntity>(_dbContext.People, rawFilters);

        // Assert
        Assert.NotEmpty(result.Data);
        Assert.Equal(0, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.All(result.Data, p => Assert.Equal(name, p.FirstName));
    }

    [Fact]
    public void ExecuteQuery_WithFilters_ReturnsFilteredData()
    {
        //Arrange
        var name = _dbContext.People.First().FirstName;
        Dictionary<string, string[]> rawFilters = new()
        {
            {"page", ["0"] },
            {"pageSize", ["10"] },
            {"firstName", [$"eq:{name}"]}
        };

        // Act
        var result = _queryService.Execute<PersonDto, PersonEntity>(_dbContext.People, rawFilters);

        // Assert
        Assert.NotEmpty(result.Data);
        Assert.Equal(0, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.All(result.Data, p => Assert.Equal(name, p.FirstName));
    }
}
