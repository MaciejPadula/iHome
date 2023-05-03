using iHome.Core.Models;

namespace iHome.Core.Services.Schedules;

public interface IScheduleService
{
    Task<IEnumerable<ScheduleModel>> GetSchedules(string userId);
    Task<ScheduleModel> GetSchedule(Guid scheduleId, string userId);
    Task<ScheduleModel> GetScheduleWithDevices(Guid scheduleId, string userId);
    Task<int> GetDevicesInScheduleCount(Guid scheduleId, string userId);

    Task AddSchedule(string scheduleName, int day, int hour, int minute, string userId);
    Task UpdateScheduleTime(Guid scheduleId, int day, int hour, int minute, string userId);
    Task RemoveSchedule(Guid scheduleId, string userId);

    Task<ScheduleDeviceModel> GetScheduleDevice(Guid scheduleId, Guid deviceId, string userId);
    Task<IEnumerable<ScheduleDeviceModel>> GetScheduleDevices(Guid scheduleId, string userId);
    Task AddOrUpdateDeviceSchedule(Guid scheduleId, Guid deviceId, string deviceData, string userId);
    Task RemoveDeviceSchedule(Guid scheduleId, Guid deviceId, string userId);
}
