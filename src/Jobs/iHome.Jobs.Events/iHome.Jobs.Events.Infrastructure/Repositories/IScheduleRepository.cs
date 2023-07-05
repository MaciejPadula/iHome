using iHome.Jobs.Events.Infrastructure.Models;

namespace iHome.Jobs.Events.Infrastructure.Repositories;

public interface IScheduleRepository
{
    IEnumerable<Schedule> GetSchedulesWithDevicesExcluding(IEnumerable<Guid> schedulesToExclude);
    Task<IEnumerable<ScheduleDevice>> GetScheduleDevices(Guid scheduleId);
}
