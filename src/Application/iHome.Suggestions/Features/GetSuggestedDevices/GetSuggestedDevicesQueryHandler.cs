using iHome.Microservices.OpenAI.Contract;
using iHome.Microservices.OpenAI.Contract.Models;
using iHome.Model;
using iHome.Shared.Logic;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Suggestions.Features.GetSuggestedDevices;

public class GetSuggestedDevicesQueryHandler : IAsyncQueryHandler<GetSuggestedDevicesQuery>
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
            Devices = GetDeviceDetails(query.Devices)
        });

        query.Result = suggested?
            .DevicesIds?
            .ToList() ?? [];
    }

    private List<DeviceDetails> GetDeviceDetails(IEnumerable<DeviceDto> devices)
    {
        return devices
            .Select(d => new DeviceDetails
            {
                Id = d.Id,
                Name = d.Name,
                Type = d.Type.ToString()
            })
            .ToList();
    }
}
