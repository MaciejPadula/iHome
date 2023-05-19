using iHome.Core.Models;

namespace iHome.Core.Repositories.Schedules;

public interface IScheduleDeviceRepository
{
    Task<List<ScheduleDeviceModel>> GetByScheduleId(Guid scheduleId);
    Task<ScheduleDeviceModel> GetByIdAndScheduleId(Guid deviceId, Guid scheduleId);

    Task Upsert(Guid scheduleId, Guid deviceId, string deviceData);
    Task Remove(Guid scheduleId, Guid deviceId);

    Task<int> CountByScheduleId(Guid scheduleId);
}
