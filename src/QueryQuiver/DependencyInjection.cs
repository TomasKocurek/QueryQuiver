using Microsoft.Extensions.DependencyInjection;
using QueryQuiver.Interfaces;
using QueryQuiver.Mapping;
using QueryQuiver.Query;
using System.Reflection;

namespace QueryQuiver;

public static class DependencyInjection
{
    public static IServiceCollection AddQueryQuiver(this IServiceCollection services)
    {
        services.AddScoped(typeof(IFilteringService<,>), typeof(FilteringService<,>));
        return services;
    }

    public static IServiceCollection AddMappingProfiles(this IServiceCollection service, Assembly assembly)
    {
        var mappingProfileType = typeof(MappingProfile<,>);
        var profiles = assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == mappingProfileType);

        foreach (var profile in profiles)
        {
            service.AddSingleton(profile.BaseType!, profile);
        }

        return service;
    }
}
