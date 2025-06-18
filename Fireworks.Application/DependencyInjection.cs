using System.Reflection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Fireworks.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        services.AddMapster();
        return services;
    }
}