using iHome.Model;

namespace iHome.Repository;

public interface IScheduleSuggestionsProvider
{
    Task<TimeModel> GetTime(string scheduleName);
    Task<IEnumerable<Guid>> GetDevices(string scheduleName, TimeModel scheduleTime);
}
