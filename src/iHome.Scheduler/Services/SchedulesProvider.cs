using Cronos;
using iHome.Scheduler.Infrastructure.Services;
using iHome.Scheduler.Repositories.Models;

namespace iHome.Scheduler.Services;

public interface ISchedulesProvider
{
    Task<IEnumerable<Schedule>> GetSchedulesToRun(DateTime now);
}

public class SchedulesProvider : ISchedulesProvider
{
    private readonly ISchedulesService _schedulesService;

    public SchedulesProvider(ISchedulesService schedulesService)
    {
        _schedulesService = schedulesService;
    }

    public async Task<IEnumerable<Schedule>> GetSchedulesToRun(DateTime now)
    {
        var schedules = await _schedulesService.GetToRunSchedules(cron =>
        {
            var expression = CronExpression.Parse(cron);
            var nextUtc = expression.GetNextOccurrence(now);
        });

        return schedules.Where(s => s.ScheduleDevices.Any());
    }
}
