using Fireworks.Domain.Common;

namespace Fireworks.Domain.Entities;

public class AuditLog : BaseEntity<Guid>
{
    public Guid? UserId { get; set; }
    public string? UserName { get; set; } = string.Empty;

    public string HttpMethod { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;

    public string RequestData { get; set; } = string.Empty;
    public string ResponseData { get; set; } = string.Empty;
    public int StatusCode { get; set; }

    public long ExecutionDurationMs { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}