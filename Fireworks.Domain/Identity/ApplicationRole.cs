using Fireworks.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Fireworks.Domain.Identity;

public class ApplicationRole:IdentityRole<Guid>
{
    public string? Description { get; set; } = string.Empty;
    public IEnumerable<ApplicationUserRole>? UserRoles { get; set; } = new List<ApplicationUserRole>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}