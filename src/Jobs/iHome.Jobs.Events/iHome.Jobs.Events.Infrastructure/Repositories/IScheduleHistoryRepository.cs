namespace iHome.Jobs.Events.Infrastructure.Repositories;

public interface IScheduleHistoryRepository
{
    Task AddRunnedSchedules(IEnumerable<Guid> scheduleIds, DateTime runDate);
}
