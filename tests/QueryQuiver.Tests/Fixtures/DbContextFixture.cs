using Microsoft.EntityFrameworkCore;
using QueryQuiver.Tests.Mocks;

namespace QueryQuiver.Tests.Fixtures;
public class DbContextFixture : IDisposable
{
    public TestDbContext DbContext { get; private set; }

    public DbContextFixture()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        DbContext = new TestDbContext(options);
        DbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
}

[CollectionDefinition(nameof(DbContextCollection))]
public class DbContextCollection : ICollectionFixture<DbContextFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
