using Microsoft.Extensions.DependencyInjection;
using QueryQuiver.Tests.Models;
using QueryQuiver.Tests.Profiles;

namespace QueryQuiver.Tests.Fixtures;
public class ServiceProviderFixture
{
    public IServiceProvider ServiceProvider { get; }

    public ServiceProviderFixture()
    {
        var services = new ServiceCollection();

        services.AddSingleton<MapProfile<PersonEntity, PersonEntity>>(new PersonTestProfile());
        services.AddQueryQuiver();

        ServiceProvider = services.BuildServiceProvider();
    }

}
