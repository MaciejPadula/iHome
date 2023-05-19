using iHome.Core.Models;

namespace iHome.Core.Repositories.Schedules;

public interface IScheduleRepository
{
    Task Add(string scheduleName, int hour, int minute, string userId);

    Task<List<ScheduleModel>> GetByUserId(string userId);
    Task<ScheduleModel> GetById(Guid scheduleId);
    Task<ScheduleModel> GetByIdWithDevices(Guid scheduleId);

    Task<int> CountByUserId(string userId);
    Task UpdateTime(Guid scheduleId, int hour, int minute);

    Task Remove(Guid scheduleId);
}
