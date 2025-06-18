using Fireworks.Api.Common.Configurations;
using Fireworks.Application;
using Fireworks.Infrastructure;

namespace Fireworks.Api.Common.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddSwaggerServices()
            .AddInfrastructureServices(configuration)
            .AddApplicationLayer();

        return services;
    }
}