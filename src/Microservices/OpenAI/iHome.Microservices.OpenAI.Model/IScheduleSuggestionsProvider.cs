using iHome.Microservices.OpenAI.Contract.Models;

namespace iHome.Microservices.OpenAI.Model;

public interface IScheduleSuggestionsProvider
{
    Task<IEnumerable<Guid>> GetDevicesIdsForSchedule(string scheduleName, string scheduleTime, IEnumerable<DeviceDetails> devices);
    Task<string> GetTimeForSchedule(string scheduleName);
}
