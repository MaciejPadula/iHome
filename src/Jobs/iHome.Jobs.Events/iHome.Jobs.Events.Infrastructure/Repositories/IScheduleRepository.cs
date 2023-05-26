using iHome.Jobs.Events.Infrastructure.Models;

namespace iHome.Jobs.Events.Infrastructure.Repositories;

public interface IScheduleRepository
{
    Task<IEnumerable<ScheduleDevice>> GetScheduleDevices(Guid scheduleId);
    Task<IEnumerable<Schedule>> GetAllSchedules();
    Task<IEnumerable<Schedule>> GetToRunSchedules(Func<int, int, bool> cronComparer);
    Task AddRunnedSchedules(IEnumerable<Guid> scheduleIds);
}
