using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QueryQuiver.Query;
using QueryQuiver.Tests.Fixtures;
using QueryQuiver.Tests.Mocks;
using QueryQuiver.Tests.Models.Dtos;
using QueryQuiver.Tests.Models.Entities;

namespace QueryQuiver.Tests;
[Collection(nameof(QueryQuiverCollection))]
public class MapProfileTests(DbContextFixture DbContextFixture, ServiceProviderFixture ServiceProviderFixture)
{
    private readonly TestDbContext _dbContext = DbContextFixture.DbContext;
    private readonly QueryService<OrderDto, OrderEntity> _queryService = ServiceProviderFixture.ServiceProvider.GetRequiredService<QueryService<OrderDto, OrderEntity>>();

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
        var result = await _queryService.ExecuteAsync(_dbContext.Orders, rawFilters);

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
        var result = await _queryService.ExecuteAsync(_dbContext.Orders.Include(o => o.Customer), rawFilters);

        // Assert
        Assert.NotEmpty(result.Data);
        Assert.All(result.Data, o => Assert.Equal(email, o.Customer.Email));
    }
}
