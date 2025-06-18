namespace Fireworks.Api.Common.interfaces;

public interface IEndpointRegistrar
{
    void MapEndpoints(IEndpointRouteBuilder endpoints);
}