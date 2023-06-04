using iHome.Jobs.Events.Infrastructure.Helpers;
using iHome.Jobs.Events.Infrastructure.Models;
using iHome.Jobs.Events.Infrastructure.Repositories;

namespace iHome.Jobs.Events.Services;

public interface ISchedulesProvider
{
    Task<IEnumerable<Schedule>> GetSchedulesToRun();
    Task AddToRunned(IEnumerable<Schedule> schedules);
}

public class SchedulesProvider : ISchedulesProvider
{
    private readonly IScheduleRepository _schedulesService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public SchedulesProvider(IScheduleRepository schedulesService, IDateTimeProvider dateTimeProvider)
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

        var schedules = await _schedulesService.GetToRunSchedules((hour, minute) =>
        {
            var todayOccurence = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, hour, minute, 0);
            return todayOccurence.EarlierThan(utcNow);
        });

        return schedules;
    }
}
