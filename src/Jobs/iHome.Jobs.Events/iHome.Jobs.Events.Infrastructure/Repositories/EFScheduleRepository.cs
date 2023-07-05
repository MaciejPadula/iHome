using iHome.Jobs.Events.Infrastructure.Contexts;
using iHome.Jobs.Events.Infrastructure.Helpers;
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

    public async Task<IEnumerable<Schedule>> GetAllSchedules()
    {
        return await _context.Schedules.ToListAsync();
    }

    public async Task<IEnumerable<ScheduleDevice>> GetScheduleDevices(Guid scheduleId)
    {
        var schedule = await _context.Schedules
            .Where(s => s.Id == scheduleId)
            .Include(s => s.ScheduleDevices)
            .SingleOrDefaultAsync();

        return schedule?.ScheduleDevices ?? Enumerable.Empty<ScheduleDevice>();
    }

    public IEnumerable<Guid> GetTodayRunnedSchedules(DateTime utcNow)
    {
        return _context.SchedulesRunHistory
            .Where(s => DateTime.Compare(utcNow.StartOfDay(), s.RunDate) < 0)
            .Select(s => s.ScheduleId);
    }

    public IEnumerable<Schedule> GetNotRunnedSchedules(IEnumerable<Guid> schedulesToSkip)
    {
        return _context.Schedules
            .Include(s => s.ScheduleDevices)
            .Where(s => s.ScheduleDevices.Any())
            .Where(s => !schedulesToSkip.Any(id => id == s.Id));
    }

    public async Task AddRunnedSchedules(IEnumerable<Guid> scheduleIds, DateTime runDate)
    {
        foreach (var scheduleId in scheduleIds)
        {
            _context.Add(new ScheduleRunHistory
            {
                Id = Guid.NewGuid(),
                ScheduleId = scheduleId,
                RunDate = runDate
            });
        }

        await _context.SaveChangesAsync();
    }
}
