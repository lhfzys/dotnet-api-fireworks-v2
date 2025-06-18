using Fireworks.Domain.Common;
using Fireworks.Domain.Identity;

namespace Fireworks.Domain.Entities;

public class RolePermission:BaseEntity<Guid>
{
    public Guid RoleId { get; set; }
    public ApplicationRole Role { get; set; } = null!;
    
    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; } = null!;
}