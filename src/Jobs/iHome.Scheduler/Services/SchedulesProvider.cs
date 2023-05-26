using Cronos;
using iHome.Infrastructure.SQL.Models.RootTables;
using iHome.Scheduler.Infrastructure.Helpers;
using iHome.Scheduler.Infrastructure.Helpers.DateTimeProvider;
using iHome.Scheduler.Infrastructure.Services.SchedulesService;

namespace iHome.Scheduler.Services;

public interface ISchedulesProvider
{
    Task<IEnumerable<Schedule>> GetSchedulesToRun();
    Task AddToRunned(IEnumerable<Schedule> schedules);
}

public class SchedulesProvider : ISchedulesProvider
{
    private readonly ISchedulesService _schedulesService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public SchedulesProvider(ISchedulesService schedulesService, IDateTimeProvider dateTimeProvider)
    {
        _schedulesService = schedulesService;
        _dateTimeProvider = dateTimeProvider;
    }

    public Task AddToRunned(IEnumerable<Schedule> schedules)
    {
        return _schedulesService.AddRunnedSchedules(schedules.Select(s => s.Id));
    }

    public async Task<IEnumerable<Schedule>> GetSchedulesToRun()
    {
        var utcNow = _dateTimeProvider.UtcNow;

        var schedules = await _schedulesService.GetToRunSchedules(cron =>
        {
            var expression = CronExpression.Parse(cron);
            var nextUtc = expression.GetOccurrences(utcNow.StartOfDay(), utcNow.EndOfDay(), true, true)
                .FirstOrDefault();

            return nextUtc.EarlierThan(utcNow);
        });

        return schedules;
    }
}
