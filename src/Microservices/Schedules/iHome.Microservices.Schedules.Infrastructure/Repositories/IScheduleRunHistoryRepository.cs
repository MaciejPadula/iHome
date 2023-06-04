namespace iHome.Microservices.Schedules.Infrastructure.Repositories;

public interface IScheduleRunHistoryRepository
{
    Task<bool> ScheduleRunned(Guid scheduleId, DateTime startOfToday);
}
