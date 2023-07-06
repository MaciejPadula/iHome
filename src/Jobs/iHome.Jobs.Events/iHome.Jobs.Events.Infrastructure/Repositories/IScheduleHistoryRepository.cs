namespace iHome.Jobs.Events.Infrastructure.Repositories;

public interface IScheduleHistoryRepository
{
    IEnumerable<Guid> GetTodayRunnedSchedules(DateTime utcNow);
    Task AddRunnedSchedules(IEnumerable<Guid> scheduleIds, DateTime runDate);
}
