using iHome.Model;

namespace iHome.Core.Services.Validation.Validators
{
    public class AllowAllValidator : IValidator
    {
        public bool IsMatched(ValidatorType type) => false;

        public Task Validate(Guid id, string userId, Func<Task> action) => action();

        public Task<T> Validate<T>(Guid id, string userId, Func<Task<T>> action) => action();
    }
}
