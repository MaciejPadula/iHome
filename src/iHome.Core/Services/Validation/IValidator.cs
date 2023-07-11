using iHome.Core.Models;

namespace iHome.Core.Services.Validation;

public interface IValidator
{
    Task Validate(Guid id, string userId, Task action);
    Task<T> Validate<T>(Guid id, string userId, Task<T> action);
    bool IsMatched(ValidatorType type);
}
