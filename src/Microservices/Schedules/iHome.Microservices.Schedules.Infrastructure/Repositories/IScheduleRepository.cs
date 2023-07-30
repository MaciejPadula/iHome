using iHome.Microservices.Schedules.Contract.Models;

namespace iHome.Microservices.Schedules.Infrastructure.Repositories;

public interface IScheduleRepository
{
    Task Add(string scheduleName, int hour, int minute, string userId);

    Task<IEnumerable<ScheduleModel>> GetByUserId(string userId);
    Task<IEnumerable<ScheduleModel>> GetByDevicesIds(IEnumerable<Guid> deviceIds);
    Task<ScheduleModel> GetById(Guid scheduleId);

    Task<int> CountByUserId(string userId);
    Task UpdateTime(Guid scheduleId, int hour, int minute);

    Task Remove(Guid scheduleId);
}
