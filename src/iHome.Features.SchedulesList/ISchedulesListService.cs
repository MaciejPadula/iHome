using iHome.Model;

namespace iHome.Features.SchedulesList;

public interface ISchedulesListService
{
    Task<List<ScheduleDto>> GetUserSchedulesOrdered(string userId);
    Task UpdateScheduleTime(Guid id, string time, int day, string userId);
    Task RemoveSchedule(Guid id, string userId);
}
