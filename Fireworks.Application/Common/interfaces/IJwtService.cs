using Fireworks.Domain.Identity;

namespace Fireworks.Application.Common.interfaces;

public interface IJwtService
{
    string GenerateAccessToken(ApplicationUser user, IList<string> roles);
    (string RefreshToken, DateTime Expires) GenerateRefreshToken();
}