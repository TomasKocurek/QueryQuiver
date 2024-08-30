using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace QueryQuiver.Tests.Fixtures;
public class ServiceProviderFixture
{
    public IServiceProvider ServiceProvider { get; }

    public ServiceProviderFixture()
    {
        var services = new ServiceCollection();

        services.AddQueryQuiver();
        services.AddMappingProfiles(Assembly.GetExecutingAssembly());

        ServiceProvider = services.BuildServiceProvider();
    }

}
