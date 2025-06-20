using Fireworks.Api.Common.Extensions;
using Fireworks.Api.Common.interfaces;
using Fireworks.Application.Features.system.Auth.Login;
using MediatR;

namespace Fireworks.Api.Endpoints;

public class AuthEndpoints : IEndpointRegistrar
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroupWithTags("/api/auth", "Auth");

        group.MapPost("/login", async (IMediator mediator, LoginRequest request)
            => (await mediator.Send(request)).ToApiResult());
    }
}