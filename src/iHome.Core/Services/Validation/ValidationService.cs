using iHome.Core.Models;
using iHome.Core.Services.Validation.Validators;

namespace iHome.Core.Services.Validation;

public interface IValidationService
{
    Task<T> Validate<T>(Guid id, string userId, ValidatorType validatorType, Task<T> action);
    Task Validate(Guid id, string userId, ValidatorType validatorType, Task action);
}

public class ValidationService : IValidationService
{
    private readonly IEnumerable<IValidator> _validators;
    private readonly AllowAllValidator _allowAllValidator;

    public ValidationService(IEnumerable<IValidator> validators)
    {
        _validators = validators;
        _allowAllValidator = new AllowAllValidator();
    }

    public Task<T> Validate<T>(Guid id, string userId, ValidatorType validatorType, Task<T> action)
    {
        var validator = TryGetValidator(validatorType);
        return validator.Validate(id, userId, action);
    }

    public Task Validate(Guid id, string userId, ValidatorType validatorType, Task action)
    {
        var validator = TryGetValidator(validatorType);
        return validator.Validate(id, userId, action);
    }

    private IValidator TryGetValidator(ValidatorType validatorType)
    {
        return _validators
            .FirstOrDefault(v => v.IsMatched(validatorType)) ?? _allowAllValidator;
    }
}
