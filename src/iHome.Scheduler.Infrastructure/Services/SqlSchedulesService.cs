using iHome.Infrastructure.SQL.Contexts;
using iHome.Infrastructure.SQL.Models;
using iHome.Scheduler.Infrastructure.Helpers;
using iHome.Scheduler.Infrastructure.Helpers.DateTimeProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace iHome.Scheduler.Infrastructure.Services;

public class SqlSchedulesService : ISchedulesService
{
    private readonly SqlDataContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;

    private const int BatchSize = 10000;

    public SqlSchedulesService(SqlDataContext context, IDateTimeProvider dateTimeProvider)
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

    public Task<IEnumerable<Schedule>> GetToRunSchedules(Func<string, bool> cronComparer)
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
                .Where(s => cronComparer.Invoke(s.ActivationCron));

            if (!schedulesToTest.Any()) break;

            schedulesToRun.AddRange(schedulesToTest);
            numberOfBatch += 1;
        }


        return Task.FromResult(schedulesToRun.AsEnumerable());
    }

    public async Task AddRunnedSchedules(IEnumerable<Guid> scheduleIds)
    {
        foreach(var scheduleId in scheduleIds)
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
