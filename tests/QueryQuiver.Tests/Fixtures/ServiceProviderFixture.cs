﻿using Microsoft.Extensions.DependencyInjection;
using QueryQuiver.Mapping;
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

        services.AddSingleton<MappingProfile<PersonDto, PersonEntity>>(new PersonTestProfile());
        services.AddSingleton<MappingProfile<OrderDto, OrderEntity>>(new OrderTestProfile());
        services.AddQueryQuiver();

        ServiceProvider = services.BuildServiceProvider();
    }

}