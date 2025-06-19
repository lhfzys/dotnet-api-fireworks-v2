namespace Fireworks.Api.Common.Extensions;

public static class SecurityHeadersExtensions
{
    public static IApplicationBuilder UseSecureHeaders(this IApplicationBuilder app)
    {
        var policy = new HeaderPolicyCollection()
            .AddDefaultSecurityHeaders()
            .AddContentSecurityPolicy(builder =>
            {
                builder.AddObjectSrc().None();
                builder.AddFormAction().Self();
                builder.AddFrameAncestors().None();
                builder.AddScriptSrc().Self().UnsafeInline();
            });

        return app.UseSecurityHeaders(policy);
    }
}