using iHome.Model;

namespace iHome.Repository;

public interface IScheduleDeviceRepository
{
    Task Add(ScheduleDeviceDto scheduleDevice);
    Task<IEnumerable<ScheduleDeviceDto>> GetScheduleDevices(Guid scheduleId);
    Task<IEnumerable<Guid>> GetUserScheduleDevices(string userId);
    Task UpdateScheduleDeviceData(ScheduleDeviceDto scheduleDevice);
    Task Remove(Guid deviceId, Guid scheduleId);
}
