using iHome.Core.Models;
using iHome.Infrastructure.SQL.Models;

namespace iHome.Core.Services.Schedules;

public interface IScheduleService
{
    Task<IEnumerable<Schedule>> GetSchedules(string userId);
    Task<Schedule> GetSchedule(Guid scheduleId, string userId);
    Task<int> GetDevicesInScheduleCount(Guid scheduleId, string userId);
    Task AddSchedule(string scheduleName, int day, int hour, int minute, string userId);
    Task UpdateScheduleTime(Guid scheduleId, int day, int hour, int minute, string userId);
    Task RemoveSchedule(Guid scheduleId, string userId);

    Task<ScheduleDevice> GetScheduleDevice(Guid scheduleId, Guid deviceId, string userId);
    Task<IEnumerable<ScheduleDevice>> GetScheduleDevices(Guid scheduleId, string userId);
    Task AddOrUpdateDeviceSchedule(Guid scheduleId, Guid deviceId, string deviceData, string userId);
    Task RemoveDeviceSchedule(Guid scheduleId, Guid deviceId, string userId);
}
