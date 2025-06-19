using Fireworks.Application.Common.interfaces;
using Fireworks.Domain.Entities;
using Fireworks.Domain.Identity;
using IPTools.Core;
using Microsoft.AspNetCore.Http;
using UAParser;

namespace Fireworks.Infrastructure.Services;

public class LoginLoggingService(IApplicationDbContext db, IClientIpService ipService, IHttpContextAccessor accessor):ILoginLoggingService
{
    public async Task LogAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        var ip = ipService.GetClientIp();
        var location = "";
        try
        {
            var ipInfo = IpTool.Search(ip);
            location = $"{ipInfo.Country}-{ipInfo.Province}-{ipInfo.City}";
        }
        catch (Exception ex)
        {
            location = "未知位置";
        }

        var userAgent = accessor.HttpContext?.Request.Headers.UserAgent.ToString() ?? "unknown";
        var uaParser = Parser.GetDefault();
        var c = uaParser.Parse(userAgent);
        var log = new UserLoginLog
        {
            UserId = user.Id,
            IpAddress = ip,
            UserAgent = userAgent,
            Device = c.Device.Family ?? "未知设备",
            Location = location
        };

        db.UserLoginLogs.Add(log);
        await db.SaveChangesAsync(cancellationToken);
    }
}