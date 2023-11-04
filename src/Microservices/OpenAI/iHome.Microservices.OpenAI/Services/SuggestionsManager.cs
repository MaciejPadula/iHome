using iHome.Microservices.OpenAI.Contract.Models.Request;
using iHome.Microservices.OpenAI.Contract.Models.Response;
using iHome.Microservices.OpenAI.Model;

namespace iHome.Microservices.OpenAI.Services;

public interface ISuggestionsManager
{
    Task<GetDevicesThatCouldMatchScheduleResponse> GetDevicesForSchedule(GetDevicesThatCouldMatchScheduleRequest request);
    Task<GetSuggestedTimeByScheduleNameResponse> GetSuggestedTime(GetSuggestedTimeByScheduleNameRequest request);
}

public class SuggestionsManager : ISuggestionsManager
{
    private readonly IScheduleSuggestionsProvider _scheduleSuggestionsProvider;

    public SuggestionsManager(IScheduleSuggestionsProvider scheduleSuggestionsProvider)
    {
        _scheduleSuggestionsProvider = scheduleSuggestionsProvider;
    }

    public async Task<GetDevicesThatCouldMatchScheduleResponse> GetDevicesForSchedule(GetDevicesThatCouldMatchScheduleRequest request)
    {
        return new()
        {
            DevicesIds = await _scheduleSuggestionsProvider.GetDevicesIdsForSchedule(
                request.ScheduleName, request.ScheduleTime, request.Devices)
        };
    }

    public async Task<GetSuggestedTimeByScheduleNameResponse> GetSuggestedTime(GetSuggestedTimeByScheduleNameRequest request)
    {
        return new()
        {
            Time = await _scheduleSuggestionsProvider.GetTimeForSchedule(
                request.ScheduleName)
        };
    }
}
