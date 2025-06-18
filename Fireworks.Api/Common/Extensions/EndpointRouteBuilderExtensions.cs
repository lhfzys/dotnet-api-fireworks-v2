namespace Fireworks.Api.Common.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static RouteGroupBuilder MapGroupWithTags(this IEndpointRouteBuilder builder, string prefix, string tag)
    {
        return builder.MapGroup(prefix).WithTags(tag);
    }
}