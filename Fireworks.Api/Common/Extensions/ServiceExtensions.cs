using Fireworks.Api.Common.Configurations;
using Fireworks.Application;
using Fireworks.Application.Common.interfaces;
using Fireworks.Domain.Common.settings;
using Fireworks.Infrastructure;
using Fireworks.Infrastructure.Services;

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
            .AddAuthorizationServices(configuration)
            .AddInfrastructureServices(configuration)
            .AddApplicationLayer()
            .AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("PostgresSqlConnection") ?? string.Empty)
            .AddRedis(configuration.GetConnectionString("Redis:ConnectionString") ?? string.Empty);
        return services;
    }
}