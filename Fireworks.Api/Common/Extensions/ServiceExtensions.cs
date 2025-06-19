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
            .AddRateLimiting()
            .AddSwaggerServices()
            .AddInfrastructureServices(configuration)
            .AddApplicationLayer()
            .AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("PostgresSqlConnection") ?? string.Empty)
            .AddRedis(configuration.GetConnectionString("Redis:ConnectionString") ?? string.Empty);

        return services;
    }
}