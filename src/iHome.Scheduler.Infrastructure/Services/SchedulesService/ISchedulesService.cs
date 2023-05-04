using iHome.Infrastructure.SQL.Models.ConnectionTables;
using iHome.Infrastructure.SQL.Models.RootTables;

namespace iHome.Scheduler.Infrastructure.Services.SchedulesService;

public interface ISchedulesService
{
    Task<IEnumerable<ScheduleDevice>> GetScheduleDevices(Guid scheduleId);
    Task<IEnumerable<Schedule>> GetAllSchedules();
    Task<IEnumerable<Schedule>> GetToRunSchedules(Func<string, bool> cronComparer);
    Task AddRunnedSchedules(IEnumerable<Guid> scheduleIds);
}
