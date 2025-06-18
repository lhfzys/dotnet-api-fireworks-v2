using FluentResults;
using MediatR;

namespace Fireworks.Application.Features.system.Users.CreateUser;

public record CreateUserRequest(string UserName, string Password) : IRequest<Result<Guid>>;