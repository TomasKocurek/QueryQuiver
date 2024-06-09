using QueryQuiver.Tests.Fixtures;
using QueryQuiver.Tests.Mocks;
using QueryQuiver.Tests.Models;

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
        var expected = GetPaginatedPeople(0, 20);
        Assert.Equal(expected, result);
        Assert.NotEmpty(result);
    }

    private List<PersonEntity> GetPaginatedPeople(int page, int pageSize)
        => [.. dbContext.People.Take(pageSize).Skip(page * pageSize)];
}
