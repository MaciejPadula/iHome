using iHome.Microservices.Authorization.Contract;
using iHome.Microservices.Authorization.Contract.Models.Request;
using iHome.Microservices.Authorization.Contract.Models.Response;
using iHome.Microservices.Authorization.Managers;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.Authorization.Controllers
{
    public class ScheduleAuthController : ControllerBase, IScheduleAuthService
    {
        private readonly IScheduleManager _scheduleManager;

        public ScheduleAuthController(IScheduleManager scheduleManager)
        {
            _scheduleManager = scheduleManager;
        }

        [HttpPost]
        public async Task<AuthResponse> CanAccessSchedule([FromBody] ScheduleAuthRequest request)
        {
            return new()
            {
                HasAccess = await _scheduleManager.CanAccess(request)
            };
        }
    }
}
