using Fireworks.Domain.Common;
using Fireworks.Domain.Identity;

namespace Fireworks.Domain.Entities;

public class UserLoginLog:BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    
    public string IpAddress { get; set; } = null!;
    public string Device { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string UserAgent { get; set; } = null!;
    public DateTime LoginTime { get; set; } = DateTime.UtcNow;
}