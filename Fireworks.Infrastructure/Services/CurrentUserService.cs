using System.Security.Claims;
using Fireworks.Application.Common.interfaces;
using Microsoft.AspNetCore.Http;

namespace Fireworks.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly ClaimsPrincipal? _user = httpContextAccessor.HttpContext?.User;

    public Guid? Id =>
        _user?.FindFirstValue(ClaimTypes.NameIdentifier) is { } idStr && Guid.TryParse(idStr, out var id)
            ? id
            : null;

    public string? UserName =>
        _user?.FindFirstValue(ClaimTypes.Name);

    public List<Guid> RoleIds =>
        _user?.FindAll(ClaimTypes.Role)
            .Select(r => Guid.TryParse(r.Value, out var guid) ? guid : Guid.Empty)
            .Where(g => g != Guid.Empty)
            .ToList() ?? [];

    public List<string> PermissionCodes =>
        _user?.FindAll("permission")
            .Select(p => p.Value)
            .ToList() ?? [];

    public bool IsAuthenticated =>
        _user?.Identity?.IsAuthenticated ?? false;

    public string? IpAddress =>
        httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
}