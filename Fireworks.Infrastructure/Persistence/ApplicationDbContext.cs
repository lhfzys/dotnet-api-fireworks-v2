using System.Security.Claims;
using Fireworks.Application.Common.interfaces;
using Fireworks.Domain.Entities;
using Fireworks.Domain.Identity;
using Fireworks.Domain.Interfaces;
using Fireworks.Infrastructure.Identity.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fireworks.Infrastructure.Persistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IHttpContextAccessor httpContextAccessor)
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, IdentityUserClaim<Guid>, ApplicationUserRole,
            IdentityUserLogin<Guid>,
            IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>(options),
        IApplicationDbContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        builder.ApplyIdentityTableNamingConvention();
    }

    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<UserLoginLog> UserLoginLogs { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        var userId = GetCurrentUserId();

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is IAuditable auditable)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        auditable.Created = now;
                        auditable.CreatedBy = userId;
                        break;

                    case EntityState.Modified:
                        auditable.LastModified = now;
                        auditable.LastModifiedBy = userId;
                        break;
                    case EntityState.Detached:
                    case EntityState.Unchanged:
                    case EntityState.Deleted:
                        break;
                }
            }

            if (entry is not { Entity: ISoftDeletable deletable, State: EntityState.Deleted }) continue;
            // 软删除逻辑
            entry.State = EntityState.Modified;
            deletable.Deleted = now;
            deletable.DeletedBy = userId;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    private Guid? GetCurrentUserId()
    {
        var userIdStr = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdStr, out var userId) ? userId : null;
    }
}