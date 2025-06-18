using Fireworks.Domain.Entities;
using Fireworks.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fireworks.Application.Common.interfaces;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> Users { get; }
    DbSet<ApplicationRole> Roles { get; }
    DbSet<ApplicationUserRole> UserRoles { get; }
    DbSet<AuditLog> AuditLogs { get; }
    DbSet<Permission> Permissions { get; }
    DbSet<RolePermission> RolePermissions { get; }
    DbSet<UserLoginLog> UserLoginLogs { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}