using iHome.Infrastructure.SQL.Models;

namespace iHome.Scheduler.Infrastructure.Services;

public interface ISchedulesService
{
    Task<IEnumerable<ScheduleDevice>> GetScheduleDevices(Guid scheduleId, string userId);
    Task<IEnumerable<Schedule>> GetAllSchedules();
    Task<IEnumerable<Schedule>> GetToRunSchedules(Action<string> cronComparer);
}
