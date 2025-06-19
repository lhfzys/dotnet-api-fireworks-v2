using Fireworks.Application.Common.interfaces;
using Microsoft.AspNetCore.Http;

namespace Fireworks.Infrastructure.Services;

public class ClientIpService(IHttpContextAccessor httpContextAccessor):IClientIpService
{
    public string GetClientIp()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null)
            return "unknown";
        if (!httpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            return httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var ip = forwardedFor.FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(ip))
            return ip;
        return httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}