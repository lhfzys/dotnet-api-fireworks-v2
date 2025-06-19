using System.Diagnostics;
using Fireworks.Application.Common.interfaces;
using Fireworks.Domain.Entities;

namespace Fireworks.Api.Common.Middlewares;

public class LoggingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, IAuditService auditService, ICurrentUserService currentUser)
    {
        var stopwatch = Stopwatch.StartNew();

        context.Request.EnableBuffering();
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;

        var originalBodyStream = context.Response.Body;
        await using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        await next(context);

        stopwatch.Stop();

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        var log = new AuditLog
        {
            UserId = currentUser.Id,
            UserName = currentUser.UserName ?? "匿名",
            HttpMethod = context.Request.Method,
            Url = context.Request.Path,
            UserAgent = context.Request.Headers.UserAgent.ToString(),
            StatusCode = context.Response.StatusCode,
            RequestData = requestBody,
            ResponseData = responseText,
            IpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "未知IP",
            ExecutionDurationMs = stopwatch.ElapsedMilliseconds
        };

        await auditService.WriteAsync(log);

        await memoryStream.CopyToAsync(originalBodyStream);
    }
}