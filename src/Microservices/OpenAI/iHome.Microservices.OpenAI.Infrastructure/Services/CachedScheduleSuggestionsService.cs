using iHome.Microservices.OpenAI.Contract.Models;
using Microsoft.Extensions.Caching.Memory;

namespace iHome.Microservices.OpenAI.Infrastructure.Services
{
    public class CachedScheduleSuggestionsService : IScheduleSuggestionsService
    {
        private readonly IScheduleSuggestionsService _scheduleSuggestionsService;
        private readonly IMemoryCache _memoryCache;

        private readonly TimeSpan _cacheAvailability = TimeSpan.FromHours(10);

        private const string GetDevicesIdsKey = "GetDevicesIdsForSchedule";
        private const string GetTimeForScheduleKey = "GetTimeForSchedule";
        private const string DefaultSuggestedTime = "06:00";

        public CachedScheduleSuggestionsService(IScheduleSuggestionsService scheduleSuggestionsService, IMemoryCache memoryCache)
        {
            _scheduleSuggestionsService = scheduleSuggestionsService;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Guid>> GetDevicesIdsForSchedule(string scheduleName, string scheduleTime, IEnumerable<DeviceDetails> devices)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"{GetDevicesIdsKey}_{scheduleName}_{scheduleTime}",
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = _cacheAvailability;
                    return _scheduleSuggestionsService.GetDevicesIdsForSchedule(scheduleName, scheduleTime, devices);
                }) ?? Enumerable.Empty<Guid>();
        }

        public async Task<string> GetTimeForSchedule(string scheduleName)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"{GetTimeForScheduleKey}_{scheduleName}",
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = _cacheAvailability;
                    return _scheduleSuggestionsService.GetTimeForSchedule(scheduleName);
                }) ?? DefaultSuggestedTime;
        }
    }
}
