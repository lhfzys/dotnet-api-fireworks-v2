using Fireworks.Domain.Identity;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Fireworks.Application.Features.system.Users.CreateUser;

public class CreateUserHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<CreateUserRequest, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        if (await userManager.FindByNameAsync(request.UserName) != null)
            return Result.Fail("用户名已存在");

        var user = new ApplicationUser
        {
            UserName = request.UserName,
        };
        var result = await userManager.CreateAsync(user, request.Password);
        return !result.Succeeded ? Result.Fail(result.Errors.Select(e => e.Description)) : Result.Ok(user.Id);
    }
}