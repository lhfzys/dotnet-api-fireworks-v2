using Fireworks.Application.Common.interfaces;
using Fireworks.Domain.Entities;
using Fireworks.Infrastructure.backgroundTasks;
using Fireworks.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Fireworks.Infrastructure.Services;

public class AuditService(IBackgroundTaskQueue queue, IServiceScopeFactory scopeFactory):IAuditService
{
    public Task WriteAsync(AuditLog log)
    {
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