namespace Fireworks.Application.Common.interfaces;

public interface IAuditLogService
{
    Task LogAsync(string action, string requestData, string responseData, bool success, Guid? userId = null,
        string? userName = null);
}