using System.Net;
using System.Text.Json;
using Fireworks.Api.Common.Models;
using FluentValidation;

namespace Fireworks.Api.Common.Middlewares;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger,
    IHostEnvironment env)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            ;

            logger.LogError(ex, "Unhandled exception occurred");

            var message = env.IsDevelopment()
                ? ex.Message
                : "服务器内部错误，请稍后再试。";

            var response = new ApiResponse<string>
            {
                Success = false,
                Data = null,
                Message = message
            };
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsJsonAsync(json);
        }
    }
}