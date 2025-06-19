using System.Diagnostics;
using Fireworks.Application.Common.interfaces;
using Fireworks.Domain.Entities;

namespace Fireworks.Api.Common.Middlewares;

public class LoggingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, IAuditService auditService)
    {
        var stopwatch = Stopwatch.StartNew();

        context.Request.EnableBuffering();
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;

        var originalBodyStream = context.Response.Body;
        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        await next(context);

        context.Response.Body = originalBodyStream;
        memoryStream.Position = 0;
        var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();
        memoryStream.Position = 0;
        await memoryStream.CopyToAsync(originalBodyStream);

        stopwatch.Stop();

        var userId = context.User?.FindFirst("sub")?.Value;
        var userName = context.User?.Identity?.Name;

        var log = new AuditLog
        {
            UserId = Guid.TryParse(userId, out var uid) ? uid : null,
            UserName = userName,
            HttpMethod = context.Request.Method,
            Url = context.Request.Path,
            StatusCode = context.Response.StatusCode,
            RequestData = requestBody,
            ResponseData = responseBody,
            IpAddress = context.Connection.RemoteIpAddress?.ToString()??"未知IP",
            ExecutionDurationMs = stopwatch.ElapsedMilliseconds
        };

        await auditService.WriteAsync(log);
    }
}