namespace Fireworks.Application.Common.interfaces;

public interface ICurrentUserService
{
    Guid? Id { get; }
    string? UserName { get; }
    List<Guid> RoleIds { get; }
    List<string> PermissionCodes { get; }
    bool IsAuthenticated { get; }
    string? IpAddress { get; }
}