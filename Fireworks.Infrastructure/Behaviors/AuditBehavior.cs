using System.Diagnostics;
using System.Text.Json;
using Fireworks.Application.Common.interfaces;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fireworks.Infrastructure.Behaviors;

public class AuditBehavior<TRequest, TResponse>(
    ILogger<AuditBehavior<TRequest, TResponse>> logger,
    IAuditLogService auditLogger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var isAuditTarget = typeof(TRequest).Name.EndsWith("Command") || typeof(TRequest).Name.Contains("Create") ||
                            typeof(TRequest).Name.Contains("Update") || typeof(TRequest).Name.Contains("Delete");

        var stopwatch = Stopwatch.StartNew();
        var requestData = JsonSerializer.Serialize(request);

        try
        {
            var response = await next(cancellationToken);
            stopwatch.Stop();

            if (isAuditTarget)
            {
                var responseData = SafeSerializeResponse(response);
                await TryLogAsync(typeof(TRequest).Name, requestData, responseData, true);
            }

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            if (isAuditTarget)
            {
                await TryLogAsync(typeof(TRequest).Name, requestData, ex.Message, true);
            }

            throw;
        }
    }

    private async Task TryLogAsync(string actionName, string requestData, string responseData, bool success)
    {
        try
        {
            await auditLogger.LogAsync(actionName, requestData, responseData, success);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to write audit log for {Action}", actionName);
        }
    }

    private string SafeSerializeResponse(object? response)
    {
        if (response == null) return "null";
        try
        {
            var type = response.GetType();
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var isSuccessProp = type.GetProperty("IsSuccess");
                var valueProp = type.GetProperty("Value");
                var reasonsProp = type.GetProperty("Reasons");
                
                var isSuccess = (bool?)isSuccessProp?.GetValue(response) ?? false;
                
                if (isSuccess)
                {
                    var value = valueProp?.GetValue(response);
                    return JsonSerializer.Serialize(value);
                }
                else
                {
                    var reasons = reasonsProp?.GetValue(response);
                    return JsonSerializer.Serialize(new
                    {
                        success = false,
                        errors = reasons
                    });
                }
            }

            if (type == typeof(FluentResults.Result))
            {
                var isSuccessProp = type.GetProperty("IsSuccess");
                var reasonsProp = type.GetProperty("Reasons");
                bool isSuccess = (bool?)isSuccessProp?.GetValue(response) ?? false;
                if (isSuccess)
                {
                    return "\"Success\"";
                }
                else
                {
                    var reasons = reasonsProp?.GetValue(response);
                    return JsonSerializer.Serialize(new
                    {
                        success = false,
                        errors = reasons
                    });
                }
            }
            return JsonSerializer.Serialize(response);
        }
        catch (Exception ex)
        {
            return $"<Serialization failed: {ex.Message}>";
        }
    }
}