using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QueryQuiver.Interfaces;
using QueryQuiver.Tests.Extensions;
using QueryQuiver.Tests.Fixtures;
using QueryQuiver.Tests.Mocks;
using QueryQuiver.Tests.Models.Dtos;
using QueryQuiver.Tests.Models.Entities;

namespace QueryQuiver.Tests;
[Collection(nameof(QueryQuiverCollection))]
public class MapProfileTests(DbContextFixture DbContextFixture, ServiceProviderFixture ServiceProviderFixture)
{
    private readonly TestDbContext _dbContext = DbContextFixture.DbContext;
    private readonly IFilteringService _queryService = ServiceProviderFixture.ServiceProvider.GetRequiredService<IFilteringService>();

    [Fact]
    public async Task ExecuteQueryAsync_WithSimpleMapping_ReturnsData()
    {
        // Arrange
        var price = (await _dbContext.Orders.FirstAsync()).Price;
        Dictionary<string, string[]> rawFilters = new()
        {
            {"orderPrice", [$"eq:{price}"]}
        };

        // Act
        var result = await _queryService.ExecuteAsync<OrderDto, OrderEntity>(_dbContext.Orders, rawFilters);

        // Assert
        Assert.NotEmpty(result.Data);
        Assert.All(result.Data, o => Assert.Equal(price, o.Price));
    }

    [Fact]
    public async Task ExecuteQueryAsync_WithNestedMapping_ReturnsData()
    {
        // Arrange
        var email = (await _dbContext.Orders.Include(o => o.Customer).FirstAsync()).Customer.Email;
        Dictionary<string, string[]> rawFilters = new()
        {
            {"customerEmail", [$"eq:{email}"]}
        };

        // Act
        var result = await _queryService.ExecuteAsync<OrderDto, OrderEntity>(_dbContext.Orders.Include(o => o.Customer), rawFilters);

        // Assert
        Assert.NotEmpty(result.Data);
        Assert.All(result.Data, o => Assert.Equal(email, o.Customer.Email));
    }

    [Fact]
    public async Task ExecuteQueryAsync_WithSortMapping_ReutrnsData()
    {
        //Arrange
        Dictionary<string, string[]> rawFilters = new()
        {
            {"sort", ["orderPrice:desc"]},
            {"page", ["0"]},
            {"pageSize", ["20"] }
        };

        //Act
        var result = await _queryService.ExecuteAsync<OrderDto, OrderEntity>(_dbContext.Orders, rawFilters);

        //Assert
        var expected = _dbContext.Orders
            .OrderByDescending(o => o.Price)
            .ApplyPagination(0, 20)
            .ToList();
        Assert.NotEmpty(result.Data);
        Assert.Equal(expected, result.Data);
    }
}
