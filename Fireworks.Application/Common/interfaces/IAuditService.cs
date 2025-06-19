using Fireworks.Domain.Entities;

namespace Fireworks.Application.Common.interfaces;

public interface IAuditService
{
    Task WriteAsync(AuditLog log);
}