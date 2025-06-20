using FluentResults;

namespace Fireworks.Application.Common.Errors;

public class ValidationError:Error
{
    public ValidationError(string message) : base(message)
    {
        Metadata.Add("Type", "Validation");
    }

    public ValidationError(string message, string field) : base(message)
    {
        Metadata.Add("Type", "Validation");
        Metadata.Add("Field", field);
    }
}