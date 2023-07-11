using iHome.Core.Models;

namespace iHome.Core.Services.Validation.Validators
{
    public class AllowAllValidator : IValidator
    {
        public bool IsMatched(ValidatorType type) => false;

        public Task Validate(Guid id, string userId, Task action) => action;

        public Task<T> Validate<T>(Guid id, string userId, Task<T> action) => action;
    }
}
