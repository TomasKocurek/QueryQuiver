namespace QueryQuiver.Tests.Fixtures;

[CollectionDefinition(nameof(QueryQuiverCollection))]
public class QueryQuiverCollection : ICollectionFixture<DbContextFixture>, ICollectionFixture<ServiceProviderFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}