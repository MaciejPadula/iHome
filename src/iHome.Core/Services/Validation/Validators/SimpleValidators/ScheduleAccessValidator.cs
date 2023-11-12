using iHome.Model;
using iHome.Microservices.Authorization.Contract;
using iHome.Microservices.Authorization.Contract.Models.Response;

namespace iHome.Core.Services.Validation.Validators.SimpleValidators
{
    public class ScheduleAccessValidator : SimpleValidatorBase, IValidator
    {
        private readonly IScheduleAuthService _scheduleAuthService;

        public ScheduleAccessValidator(IScheduleAuthService scheduleAuthService)
        {
            _scheduleAuthService = scheduleAuthService;
        }

        public override bool IsMatched(ValidatorType type) =>
            type == ValidatorType.Schedule;

        protected override Task<AuthResponse> AuthorizationMethod(Guid id, string userId) =>
            _scheduleAuthService.CanAccessSchedule(new() { ScheduleId = id, UserId = userId });
    }
}
