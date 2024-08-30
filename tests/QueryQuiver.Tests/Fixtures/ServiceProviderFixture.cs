using Microsoft.Extensions.DependencyInjection;
using QueryQuiver.Tests.Models.Dtos;
using QueryQuiver.Tests.Models.Entities;
using QueryQuiver.Tests.Profiles;

namespace QueryQuiver.Tests.Fixtures;
public class ServiceProviderFixture
{
    public IServiceProvider ServiceProvider { get; }

    public ServiceProviderFixture()
    {
        var services = new ServiceCollection();

        services.AddSingleton<MapProfile<PersonDto, PersonEntity>>(new PersonTestProfile());
        services.AddSingleton<MapProfile<OrderDto, OrderEntity>>(new OrderTestProfile());
        services.AddQueryQuiver();

        ServiceProvider = services.BuildServiceProvider();
    }

}
