using Fireworks.Api.Common.Models;
using FluentResults;

namespace Fireworks.Api.Common.Extensions;

public static class ResultExtensions
{
    public static IResult ToApiResult<T>(this Result<T> result)
    {
        var validationErrors = result.Reasons
            .OfType<ValidationError>()
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.Message).ToArray()
            );

        var response = new ApiResponse<T>
        {
            Success = result.IsSuccess,
            Data = result.IsSuccess ? result.Value : default,
            Message = result.IsSuccess
                ? "操作成功"
                : result.Errors.FirstOrDefault()?.Message ?? "请求失败",
            Errors = validationErrors.Count > 0 ? validationErrors : null
        };

        return Results.Ok(response);
    }

    public static IResult ToApiResult(this Result result)
    {
        var validationErrors = result.Reasons
            .OfType<ValidationError>()
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.Message).ToArray()
            );

        var response = new ApiResponse<object>
        {
            Success = result.IsSuccess,
            Data = null,
            Message = result.IsSuccess
                ? "操作成功"
                : result.Errors.FirstOrDefault()?.Message ?? "请求失败",
            Errors = validationErrors.Count > 0 ? validationErrors : null
        };

        return Results.Ok(response);
    }
}