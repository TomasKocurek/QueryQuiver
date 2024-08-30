using Microsoft.Extensions.DependencyInjection;
using QueryQuiver.Interfaces;
using QueryQuiver.Query;

namespace QueryQuiver;

public static class DependencyInjection
{
    public static IServiceCollection AddQueryQuiver(this IServiceCollection services)
    {
        services.AddScoped(typeof(IFilteringService<,>), typeof(FilteringService<,>));
        return services;
    }
}
