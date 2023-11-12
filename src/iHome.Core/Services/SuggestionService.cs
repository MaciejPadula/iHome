using iHome.Microservices.OpenAI.Contract;
using iHome.Microservices.OpenAI.Contract.Models;
using iHome.Model;
using iHome.Shared.Logic;

namespace iHome.Core.Services;

public interface ISuggestionService
{
    Task<TimeModel> GetSuggestedTime(string scheduleName);
    Task<List<Guid>> GetSuggestedDevices(string scheduleName, string scheduleTime, string userId);
}

public class SuggestionService : ISuggestionService
{
    private readonly ISuggestionsService _suggestionsService;
    private readonly IDeviceService _deviceService;
    private readonly ITimeModelParser _parser;

    public SuggestionService(ISuggestionsService suggestionsService, ITimeModelParser parser, IDeviceService deviceService)
    {
        _suggestionsService = suggestionsService;
        _parser = parser;
        _deviceService = deviceService;
    }

    public async Task<List<Guid>> GetSuggestedDevices(string scheduleName, string scheduleTime, string userId)
    {
        var suggested = await _suggestionsService.GetDevicesThatCouldMatchSchedule(new()
        {
            ScheduleName = scheduleName,
            ScheduleTime = scheduleTime,
            Devices = await GetDeviceDetails(userId)
        });

        return suggested?.DevicesIds?.ToList() ?? Enumerable.Empty<Guid>().ToList();
    }

    public async Task<TimeModel> GetSuggestedTime(string scheduleName)
    {
        var response = await _suggestionsService.GetSuggestedTimeByScheduleName(new()
        {
            ScheduleName = scheduleName
        });

        if (string.IsNullOrEmpty(response?.Time))
        {
            throw new ArgumentNullException();
        }

        var time = _parser.Parse(response.Time);
        //var txt = response.Time.Split(":");
        //if (txt.Length != 2) return Ok(null);

        return time;
    }

    private async Task<List<DeviceDetails>> GetDeviceDetails(string userId)
    {
        var devices = (await _deviceService.GetDevicesForScheduling(userId))
            .Select(d => new DeviceDetails
            {
                Id = d.Id,
                Name = d.Name,
                Type = d.Type.ToString()
            });

        return devices.ToList();
    }
}
