using iHome.Microservices.OpenAI.Contract;
using iHome.Microservices.OpenAI.Contract.Models.Request;
using iHome.Microservices.OpenAI.Contract.Models.Response;
using iHome.Microservices.OpenAI.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.OpenAI.Controllers
{
    public class SuggestionsController : ControllerBase, ISuggestionsService
    {
        private readonly IScheduleSuggestionsService _scheduleSuggestionsService;

        public SuggestionsController(IScheduleSuggestionsService scheduleSuggestionsService)
        {
            _scheduleSuggestionsService = scheduleSuggestionsService;
        }

        [HttpPost]
        public async Task<GetDevicesThatCouldMatchScheduleResponse> GetDevicesThatCouldMatchSchedule(GetDevicesThatCouldMatchScheduleRequest request)
        {
            return new GetDevicesThatCouldMatchScheduleResponse 
            {
                DevicesIds = await _scheduleSuggestionsService.GetDevicesIdsForSchedule(request.ScheduleName, request.ScheduleTime, request.Devices)
            };
        }

        [HttpPost]
        public async Task<GetSuggestedTimeByScheduleNameResponse> GetSuggestedTimeByScheduleName(GetSuggestedTimeByScheduleNameRequest request)
        {
            return new GetSuggestedTimeByScheduleNameResponse
            {
                Time = await _scheduleSuggestionsService.GetTimeForSchedule(request.ScheduleName)
            };
        }
    }
}
