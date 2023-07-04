using iHome.Jobs.Events.Infrastructure.Models;

namespace iHome.Jobs.Events.Infrastructure.Repositories;

public interface IScheduleRepository
{
    IEnumerable<Guid> GetTodayRunnedSchedules(DateTime utcNow);
    IEnumerable<Schedule> GetNotRunnedSchedules(IEnumerable<Guid> schedulesToSkip);

    Task<IEnumerable<ScheduleDevice>> GetScheduleDevices(Guid scheduleId);
    Task<IEnumerable<Schedule>> GetAllSchedules();
    Task AddRunnedSchedules(IEnumerable<Guid> scheduleIds, DateTime runDate);
}
