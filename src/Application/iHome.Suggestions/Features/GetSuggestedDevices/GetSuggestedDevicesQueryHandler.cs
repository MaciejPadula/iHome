using iHome.Microservices.OpenAI.Contract;
using iHome.Microservices.OpenAI.Contract.Models;
using iHome.Model;
using iHome.Shared.Logic;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Suggestions.Features.GetSuggestedDevices
{
    internal class GetSuggestedDevicesQueryHandler : IAsyncQueryHandler<GetSuggestedDevicesQuery>
    {
        private readonly ISuggestionsService _suggestionsService;
        private readonly ITimeModelParser _timeModelParser;

        public GetSuggestedDevicesQueryHandler(ITimeModelParser timeModelParser, ISuggestionsService suggestionsService)
        {
            _timeModelParser = timeModelParser;
            _suggestionsService = suggestionsService;
        }

        public async Task HandleAsync(GetSuggestedDevicesQuery query)
        {
            var suggested = await _suggestionsService.GetDevicesThatCouldMatchSchedule(new()
            {
                ScheduleName = query.Name,
                ScheduleTime = query.Time.ToString(),
                Devices = await GetDeviceDetails(query.UserId)
            });

            query.Result = suggested?
                .DevicesIds?
                .ToList() ?? [];
        }

        private async Task<List<DeviceDetails>> GetDeviceDetails(string userId)
        {
            var devices = new List<DeviceDto>()//(await _deviceService.GetDevicesForScheduling(userId))
                .Select(d => new DeviceDetails
                {
                    Id = d.Id,
                    Name = d.Name,
                    Type = d.Type.ToString()
                });

            return devices.ToList();
        }
    }
}
