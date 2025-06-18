using Fireworks.Domain.Common;
using Fireworks.Domain.Entities;
using Fireworks.Domain.Identity;
using Fireworks.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Fireworks.Domain.Identity;

public class ApplicationUser : IdentityUser<Guid>,IAuditable,ISoftDeletable
{
    public bool? IsActive { get; set; } = true;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public IEnumerable<ApplicationUserRole>? UserRoles { get; set; } = new List<ApplicationUserRole>();
    public DateTimeOffset Created { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTimeOffset? Deleted { get; set; }
    public Guid? DeletedBy { get; set; }
}