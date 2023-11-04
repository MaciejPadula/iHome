using iHome.Infrastructure.Cache;
using iHome.Microservices.OpenAI.Contract.Models;
using iHome.Microservices.OpenAI.Model;

namespace iHome.Microservices.OpenAI.Infrastructure.Providers
{
    public class CachedScheduleSuggestionsProvider : IScheduleSuggestionsProvider
    {
        private readonly IScheduleSuggestionsProvider _scheduleSuggestionsService;
        private readonly ICache _cache;

        private readonly TimeSpan _cacheAvailability = TimeSpan.FromHours(10);

        private const string GetDevicesIdsKey = "GetDevicesIdsForSchedule";
        private const string GetTimeForScheduleKey = "GetTimeForSchedule";
        private const string DefaultSuggestedTime = "06:00";

        public CachedScheduleSuggestionsProvider(IScheduleSuggestionsProvider scheduleSuggestionsService, ICache cache)
        {
            _scheduleSuggestionsService = scheduleSuggestionsService;
            _cache = cache;
        }

        public async Task<IEnumerable<Guid>> GetDevicesIdsForSchedule(string scheduleName, string scheduleTime, IEnumerable<DeviceDetails> devices)
        {
            return await _cache.GetOrCreateAsync(
                $"{GetDevicesIdsKey}_{scheduleName}_{scheduleTime}",
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = _cacheAvailability;
                    return _scheduleSuggestionsService.GetDevicesIdsForSchedule(scheduleName, scheduleTime, devices);
                }) ?? Enumerable.Empty<Guid>();
        }

        public async Task<string> GetTimeForSchedule(string scheduleName)
        {
            return await _cache.GetOrCreateAsync(
                $"{GetTimeForScheduleKey}_{scheduleName}",
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = _cacheAvailability;
                    return _scheduleSuggestionsService.GetTimeForSchedule(scheduleName);
                }) ?? DefaultSuggestedTime;
        }
    }
}
