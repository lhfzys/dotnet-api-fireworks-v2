using System.Text;
using Fireworks.Domain.Common.settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Fireworks.Api.Common.Configurations;

public static class AuthorizationServiceRegistration
{
    public static IServiceCollection AddAuthorizationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings?.Issuer,
                    ValidAudience = jwtSettings?.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings?.SecretKey!))
                };
            });
        services.AddAuthorization(options =>
        {
            // foreach (var permission in PermissionConstants.All)
            // {
            //     options.AddPolicy($"Permission:{permission}", policy =>
            //         policy.RequireClaim("Permission", permission));
            // }

            options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
        });
        return services;
    }
}