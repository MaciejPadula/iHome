using iHome.Microservices.Schedules.Contract.Models;

namespace iHome.Microservices.Schedules.Infrastructure.Repositories;

public interface IScheduleDeviceRepository
{
    Task Add(Guid scheduleId, Guid deviceId, string deviceData);

    Task<IEnumerable<ScheduleDeviceModel>> GetByScheduleId(Guid scheduleId);
    Task<ScheduleDeviceModel?> GetByIdAndScheduleId(Guid deviceId, Guid scheduleId);

    Task Update(Guid scheduleId, Guid deviceId, string deviceData);
    Task Remove(Guid scheduleId, Guid deviceId);

    Task<int> CountByScheduleId(Guid scheduleId);
}
