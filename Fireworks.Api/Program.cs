using System.Reflection;
using Fireworks.Api.Common.Configurations;
using Fireworks.Api.Common.Extensions;
using Fireworks.Api.Common.interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();
app.UseHttpsRedirection();
app.UseSwaggerDocumentation();

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
app.Run();