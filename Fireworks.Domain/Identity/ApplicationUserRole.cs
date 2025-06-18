using Fireworks.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Fireworks.Domain.Identity;

public class ApplicationUserRole : IdentityUserRole<Guid>
{
    public virtual ApplicationUser User { get; set; } = null!;
    public virtual ApplicationRole Role { get; set; } = null!;
}