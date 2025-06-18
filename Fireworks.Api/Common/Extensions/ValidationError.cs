using FluentResults;

namespace Fireworks.Api.Common.Extensions;

public class ValidationError: Error
{
    public string PropertyName { get; }

    public ValidationError(string propertyName, string errorMessage)
        : base(errorMessage)
    {
        PropertyName = propertyName;
        Metadata.Add("PropertyName", propertyName);
    }
}