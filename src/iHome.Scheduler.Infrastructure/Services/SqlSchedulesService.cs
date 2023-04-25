using iHome.Infrastructure.SQL.Contexts;
using iHome.Scheduler.Repositories.Models;

namespace iHome.Scheduler.Infrastructure.Services;

public class SqlSchedulesService : ISchedulesService
{
    private readonly SqlDataContext _context;

    public SqlSchedulesService(SqlDataContext context)
    {
        _context = context;
    }

    public Task<IEnumerable<Schedule>> GetAllSchedules()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ScheduleDevice>> GetScheduleDevices(Guid scheduleId, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Schedule>> GetToRunSchedules(Action<string> cronComparer)
    {
        throw new NotImplementedException();
    }
}
