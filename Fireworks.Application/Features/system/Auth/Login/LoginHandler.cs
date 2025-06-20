using Fireworks.Application.Common.interfaces;
using Fireworks.Domain.Entities;
using Fireworks.Domain.Identity;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Fireworks.Application.Features.system.Auth.Login;

public class LoginHandler(
    UserManager<ApplicationUser> userManager,
    IJwtService jwtService,
    ILoginLoggingService loginLoggingService,
    IApplicationDbContext dbContext)
    : IRequestHandler<LoginRequest, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.UserName);
        if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            return Result.Fail("用户名或密码错误");
        }

        var roles = await userManager.GetRolesAsync(user);
        var accessToken = jwtService.GenerateAccessToken(user, roles);
        var (refreshToken, refreshExpires) = jwtService.GenerateRefreshToken();

        var tokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            Expires = refreshExpires
        };

        dbContext.RefreshTokens.Add(tokenEntity);
        await dbContext.SaveChangesAsync(cancellationToken);
        await loginLoggingService.LogAsync(user, cancellationToken);

        return Result.Ok(new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(60),
            UserId = user.Id,
            UserName = user.UserName!,
            Roles = roles.ToList()
        });
    }
}