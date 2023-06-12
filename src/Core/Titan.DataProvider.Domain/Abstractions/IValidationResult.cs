using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Abstractions;

public interface IValidationResult
{
    public static readonly Error ValidationError = new(
        "ValidationError",
        "A validation problem occurred.");

    Error[] Errors { get; }
}
