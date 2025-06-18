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
        catch (ValidationException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status200OK;

            logger.LogWarning("Validation failed: {Errors}", ex.Errors);

            var errors = ex.Errors
                .Select(e => new { field = e.PropertyName, message = e.ErrorMessage })
                .ToArray();

            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                message = "验证失败",
                data = (object?)null,
                errors
            });
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status200OK;

            logger.LogError(ex, "Unhandled exception occurred");

            var message = env.IsDevelopment()
                ? ex.Message
                : "服务器内部错误，请稍后再试。";

            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                message,
                data = (object?)null
            });
        }
    }
}