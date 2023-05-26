using iHome.Jobs.Events.Infrastructure.Contexts;
using iHome.Jobs.Events.Infrastructure.Helpers;
using iHome.Jobs.Events.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace iHome.Jobs.Events.Infrastructure.Repositories;

public class EFScheduleRepository : IScheduleRepository
{
    private readonly SqlDataContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;

    private const int BatchSize = 10000;

    public EFScheduleRepository(SqlDataContext context, IDateTimeProvider dateTimeProvider)
    {
        _context = context;
        _dateTimeProvider = dateTimeProvider;
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

    public Task<IEnumerable<Schedule>> GetToRunSchedules(Func<int, int, bool> cronComparer)
    {
        var numberOfBatch = 0;

        var todayRunnedSchedules = _context.SchedulesRunHistory
            .Where(s => DateTime.Compare(_dateTimeProvider.Now.Date, s.RunDate) < 0)
            .Select(s => s.ScheduleId);

        var schedules = _context.Schedules
            .Include(s => s.ScheduleDevices)
            .Where(s => s.ScheduleDevices.Any())
            .Where(s => !todayRunnedSchedules.Any(id => id == s.Id));

        var schedulesToRun = new List<Schedule>();

        while (true)
        {
            var schedulesToTest = schedules
                .OrderBy(s => s.Modified)
                .Skip(numberOfBatch * BatchSize)
                .Take(BatchSize)
                .AsEnumerable()
                .Where(s => cronComparer.Invoke(s.Hour, s.Minute));

            if (!schedulesToTest.Any()) break;

            schedulesToRun.AddRange(schedulesToTest);
            numberOfBatch += 1;
        }


        return Task.FromResult(schedulesToRun.AsEnumerable());
    }

    public async Task AddRunnedSchedules(IEnumerable<Guid> scheduleIds)
    {
        foreach (var scheduleId in scheduleIds)
        {
            _context.Add(new ScheduleRunHistory
            {
                Id = Guid.NewGuid(),
                ScheduleId = scheduleId,
                RunDate = _dateTimeProvider.UtcNow
            });
        }

        await _context.SaveChangesAsync();
    }
}
