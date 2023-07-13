using iHome.Core.Models;
using iHome.Microservices.Authorization.Contract.Models.Response;

namespace iHome.Core.Services.Validation.Validators
{
    public abstract class SimpleValidatorBase : IValidator
    {
        public abstract bool IsMatched(ValidatorType type);

        protected abstract Task<AuthResponse> AuthorizationMethod(Guid id, string userId);

        private async Task InternalAuthorization(Guid id, string userId)
        {
            var result = await AuthorizationMethod(id, userId);

            if (!result?.HasAccess ?? true)
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task Validate(Guid id, string userId, Func<Task> action)
        {
            await InternalAuthorization(id, userId);
            await action();
        }

        public async Task<T> Validate<T>(Guid id, string userId, Func<Task<T>> action)
        {
            await InternalAuthorization(id, userId);
            return await action();
        }
    }
}
