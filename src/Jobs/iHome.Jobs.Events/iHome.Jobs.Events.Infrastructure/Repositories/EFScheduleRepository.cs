using iHome.Jobs.Events.Infrastructure.Contexts;
using iHome.Jobs.Events.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace iHome.Jobs.Events.Infrastructure.Repositories;

public class EFScheduleRepository : IScheduleRepository
{
    private readonly SqlDataContext _context;

    public EFScheduleRepository(SqlDataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ScheduleDevice>> GetScheduleDevices(Guid scheduleId)
    {
        var schedule = await _context.Schedules
            .Where(s => s.Id == scheduleId)
            .Include(s => s.ScheduleDevices)
            .SelectMany(s => s.ScheduleDevices)
            .ToListAsync();

        return schedule ?? Enumerable.Empty<ScheduleDevice>();
    }

    public IEnumerable<Schedule> GetSchedulesWithDevicesExcluding(IEnumerable<Guid> schedulesToExclude)
    {
        return _context.Schedules
            .Include(s => s.ScheduleDevices)
            .Where(s => s.ScheduleDevices.Any())
            .Where(s => !schedulesToExclude.Any(id => id == s.Id));
    }
}
