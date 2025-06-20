using Fireworks.Application.Common.Errors;
using FluentResults;
using FluentValidation;
using MediatR;
using FluentValidation.Results;

namespace Fireworks.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var failures = validators
            .Select(v => v.Validate(context))
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count == 0) return await next(cancellationToken);
        // 如果你的 Handler 返回 Result<T> 类型：
        var errors = failures
            .Select(e => new ValidationError(e.ErrorMessage)
                .WithMetadata("Field", e.PropertyName))
            .ToList();

        var resultType = typeof(TResponse);
        var failed = typeof(Result<>)
            .MakeGenericType(resultType.GenericTypeArguments[0])
            .GetMethod("Fail", [typeof(List<ValidationError>)])!
            .Invoke(null, [errors]);

        return (TResponse)failed!;

    }
}
