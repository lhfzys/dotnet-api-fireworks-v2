using Fireworks.Domain.Common;
using Fireworks.Shared.Permissions;

namespace Fireworks.Domain.Entities;

public class Permission:BaseEntity<Guid>
{
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public PermissionType Type { get; set; }
    public Guid? ParentId { get; set; }
    public Permission? Parent { get; set; }     
    public int Order { get; set; } = 0;
    public bool IsEnabled { get; set; } = true;
    public string Description { get; set; } = string.Empty;
    public string? Url { get; set; }
    public string? Icon { get; set; }
    public ICollection<Permission> Children { get; set; } = new List<Permission>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}