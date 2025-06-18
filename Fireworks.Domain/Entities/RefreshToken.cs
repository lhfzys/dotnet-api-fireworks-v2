using Fireworks.Domain.Common;
using Fireworks.Domain.Identity;

namespace Fireworks.Domain.Entities;

public class RefreshToken : BaseEntity<Guid>
{
    public string Token { get; set; } = null!;
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public string CreatedByIp { get; set; } = null!;
    public DateTime? Revoked { get; set; }
    public string? ReplacedByToken { get; set; }

    public bool IsActive => Revoked == null && !IsExpired;
    private bool IsExpired => Expires <= DateTime.UtcNow;

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
}