using System.Reflection;
using Fireworks.Application.Common.Behaviors;
using FluentValidation;
using Mapster;
using MediatR;
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
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddMapster();
        return services;
    }
}