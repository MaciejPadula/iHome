using iHome.Model;

namespace iHome.Features.AddSchedule;

public interface IAddScheduleService
{
    Task AddSchedule(string name, int day, string time, string userId);
    Task<TimeModel> GetSuggestedTime(string name);
    Task<List<DeviceDto>> GetSuggestedDevices(string name, string time, string userId);
}
