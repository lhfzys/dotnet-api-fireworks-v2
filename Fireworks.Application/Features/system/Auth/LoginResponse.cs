using Fireworks.Domain.Identity;

namespace Fireworks.Application.Features.system.Auth;

public class LoginResponse
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime AccessTokenExpiresAt { get; set; }
    
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public List<string> Roles { get; set; } = [];
    
}