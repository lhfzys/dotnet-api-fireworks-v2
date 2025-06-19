using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace Fireworks.Api.Common.Extensions;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("Fixed", opt =>
            {
                opt.Window = TimeSpan.FromSeconds(10);
                opt.PermitLimit = 5;
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                opt.QueueLimit = 2;
            });
        });

        return services;
    }
    public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder app)
    {
        return app.UseRateLimiter();
    }
}