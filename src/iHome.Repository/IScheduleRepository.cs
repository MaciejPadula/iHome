using iHome.Model;

namespace iHome.Repository;

public interface IScheduleRepository
{
    Task Add(ScheduleDto schedule);
    Task<IEnumerable<ScheduleDto>> GetUserSchedules(string userId);
    Task UpdateScheduleTime(Guid id, TimeModel time, int day);
    Task Remove(Guid id);
}
