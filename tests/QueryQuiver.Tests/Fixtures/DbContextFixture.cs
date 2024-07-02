using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using QueryQuiver.Tests.Mocks;

namespace QueryQuiver.Tests.Fixtures;
public class DbContextFixture : IDisposable
{
    public TestDbContext DbContext { get; private set; }

    private readonly IContainer dbContainer = null!;

    private const int PORT = 1455;
    private const string PASSWORD = "Password1234!";


    public DbContextFixture()
    {
        dbContainer = new ContainerBuilder()
            .WithName(Guid.NewGuid().ToString())
            .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
            .WithPortBinding(PORT, 1433)
            .WithEnvironment(new Dictionary<string, string>
            {
                { "ACCEPT_EULA", "Y" },
                { "SA_PASSWORD", PASSWORD }
            })
            .Build();

        dbContainer.StartAsync().Wait();

        var connectionString = $"Server=localhost,{PORT};Initial Catalog=TestDb;User Id=sa;Password={PASSWORD};Encrypt=false";

        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlServer(connectionString, opt => opt.EnableRetryOnFailure())
            .Options;

        DbContext = new TestDbContext(options);
        DbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
        dbContainer.DisposeAsync();
    }
}

[CollectionDefinition(nameof(DbContextCollection))]
public class DbContextCollection : ICollectionFixture<DbContextFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
