using Fireworks.Api.Common.Extensions;
using Fireworks.Api.Common.interfaces;
using Fireworks.Application.Features.system.Users.CreateUser;
using MediatR;

namespace Fireworks.Api.Common.Endpoints;

public class UserEndpoints : IEndpointRegistrar
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroupWithTags("/api/users", "Users");

        group.MapPost("/", async (IMediator mediator, CreateUserRequest request)
            => (await mediator.Send(request)).ToApiResult());
    }
}