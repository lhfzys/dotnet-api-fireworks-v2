using System.Reflection;
using Fireworks.Api.Common.Configurations;
using Fireworks.Api.Common.Extensions;
using Fireworks.Api.Common.interfaces;
using Fireworks.Api.Common.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();
app.UseSecureHeaders();
app.UseRateLimiting();
app.UseHttpsRedirection();
app.UseSwaggerDocumentation();
app.UseCustomExceptionHandling();
app.UseMiddleware<LoggingMiddleware>();
app.UseHttpsRedirection();
var endpointRegistrars = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => typeof(IEndpointRegistrar).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false })
    .Select(Activator.CreateInstance)
    .Cast<IEndpointRegistrar>();
foreach (var registrar in endpointRegistrars)
{
    registrar.MapEndpoints(app);
}

app.MapHealthChecks("/health");
app.Run();