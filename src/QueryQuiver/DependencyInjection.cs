using Microsoft.Extensions.DependencyInjection;
using QueryQuiver.Query;

namespace QueryQuiver;

public static class DependencyInjection
{
    public static IServiceCollection AddQueryQuiver(this IServiceCollection services)
    {
        services.AddScoped(typeof(QueryService<,>));
        return services;
    }
}
