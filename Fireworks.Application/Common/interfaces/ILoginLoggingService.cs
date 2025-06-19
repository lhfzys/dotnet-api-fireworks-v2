using Fireworks.Domain.Identity;

namespace Fireworks.Application.Common.interfaces;

public interface ILoginLoggingService
{
    Task LogAsync(ApplicationUser user, CancellationToken cancellationToken = default);
}