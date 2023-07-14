using iHome.Core.Models;

namespace iHome.Core.Services.Validation;

public interface IValidator
{
    Task Validate(Guid id, string userId, Func<Task> action);
    Task<T> Validate<T>(Guid id, string userId, Func<Task<T>> action);
    bool IsMatched(ValidatorType type);
}
