using Fireworks.Application.Common.interfaces;
using Fireworks.Domain.Entities;
using Fireworks.Infrastructure.backgroundTasks;
using Fireworks.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Fireworks.Infrastructure.Services;

public class AuditLogService(
    IBackgroundTaskQueue queue,
    IServiceScopeFactory scopeFactory,
    ICurrentUserService currentUser,
    IHttpContextAccessor accessor) : IAuditLogService
{
    public Task LogAsync(string action, string requestData, string responseData, bool success,
        Guid? userId = null, string? userName = null)
    {
        var context = accessor.HttpContext;
        var log = new AuditLog
        {
            UserId = userId ?? currentUser.Id,
            UserName = userName ?? currentUser.UserName ?? "Unknown",
            HttpMethod = context?.Request.Method ?? "N/A",
            Url = context?.Request.Path.Value ?? "N/A",
            Action = action,
            IpAddress = context?.Connection.RemoteIpAddress?.ToString() ?? "N/A",
            UserAgent = context?.Request.Headers["User-Agent"].ToString() ?? "N/A",
            RequestData = requestData,
            ResponseData = responseData,
            StatusCode = context?.Response.StatusCode ?? 0,
            ExecutionDurationMs = 0,
            Timestamp = DateTime.UtcNow
        };

        queue.Enqueue(async ct =>
        {
            using var scope = scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.AuditLogs.Add(log);
            await db.SaveChangesAsync(ct);
        });

        return Task.CompletedTask;
    }
}