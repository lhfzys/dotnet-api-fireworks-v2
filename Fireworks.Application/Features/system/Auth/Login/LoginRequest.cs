using FluentResults;
using MediatR;

namespace Fireworks.Application.Features.system.Auth.Login;

public class LoginRequest : IRequest<Result<LoginResponse>>
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}