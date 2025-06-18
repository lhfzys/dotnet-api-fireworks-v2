using FluentResults;
using FluentValidation.Results;

namespace Fireworks.Api.Common.Extensions;

public static class ValidationHelper
{
    public static Result<T> ToValidationResult<T>(this ValidationResult validationResult)
    {
        var result = Result.Fail<T>("验证失败");

        foreach (var error in validationResult.Errors)
        {
            result.WithError(new ValidationError(error.PropertyName, error.ErrorMessage));
        }

        return result;
    }
}
