using Fireworks.Api.Common.Models;
using FluentResults;

namespace Fireworks.Api.Common.Extensions;

public static class ResultExtensions
{
    public static IResult ToApiResult(this Result result)
    {
        if (!result.IsSuccess)
        {
            return Results.Json(new
            {
                success = true,
                data = (object?)null,
                message = result.Errors.FirstOrDefault()?.Message ?? "操作失败",
                errors = result.Reasons
                    .OfType<ValidationError>()
                    .Select(r => new
                    {
                        field = r.Metadata.TryGetValue("Field", out var field) ? field?.ToString() : null,
                        message = r.Message
                    })
                    .ToList()
            });
        }

        return Results.Json(new
        {
            success = true,
            data = (object?)null,
            message = "操作成功"
        });
    }

    public static IResult ToApiResult<T>(this Result<T> result)
    {
        if (!result.IsSuccess)
        {
            return Results.Json(new
            {
                success = false,
                data = (object?)null,
                message = result.Errors.FirstOrDefault()?.Message ?? "操作失败",
                errors = result.Reasons
                    .OfType<ValidationError>()
                    .Select(r => new
                    {
                        field = r.Metadata.TryGetValue("Field", out var field) ? field?.ToString() : null,
                        message = r.Message
                    })
                    .ToList()
            });
        }

        return Results.Json(new
        {
            success = true,
            data = result.Value,
            message = "操作成功"
        });
    }
}