using iHome.Microservices.OpenAI.Contract;
using iHome.Microservices.OpenAI.Contract.Models.Request;
using iHome.Microservices.OpenAI.Contract.Models.Response;
using iHome.Microservices.OpenAI.Model;
using iHome.Microservices.OpenAI.Services;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.OpenAI.Controllers
{
    public class SuggestionsController : ControllerBase, ISuggestionsService
    {
        private readonly ISuggestionsManager _suggestionsManager;

        public SuggestionsController(ISuggestionsManager suggestionsManager)
        {
            _suggestionsManager = suggestionsManager;
        }

        [HttpPost]
        public Task<GetDevicesThatCouldMatchScheduleResponse> GetDevicesThatCouldMatchSchedule([FromBody] GetDevicesThatCouldMatchScheduleRequest request) =>
            _suggestionsManager.GetDevicesForSchedule(request);

        [HttpPost]
        public Task<GetSuggestedTimeByScheduleNameResponse> GetSuggestedTimeByScheduleName([FromBody] GetSuggestedTimeByScheduleNameRequest request) =>
            _suggestionsManager.GetSuggestedTime(request);
    }
}
