using iHome.Jobs.Events.Infrastructure.Helpers;
using iHome.Jobs.Events.Infrastructure.Models;
using iHome.Jobs.Events.Infrastructure.Repositories;
using iHome.Jobs.Events.Scheduler.Logic;

namespace iHome.Jobs.Events.Services;

public interface ISchedulesProvider
{
    IEnumerable<Schedule> GetSchedulesToRun();
    Task AddToRunned(IEnumerable<Schedule> schedules);
    Task AddToRunned(Schedule schedule);
}

public class SchedulesProvider : ISchedulesProvider
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IScheduleHistoryRepository _historyRepository;
    private readonly IScheduleRunningConditionChecker _scheduleRunningConditionChecker;
    private readonly IDateTimeProvider _dateTimeProvider;

    private const int BatchSize = 1000;

    public SchedulesProvider(IScheduleRepository scheduleRepository, IScheduleHistoryRepository historyRepository, IDateTimeProvider dateTimeProvider, IScheduleRunningConditionChecker scheduleRunningConditionChecker)
    {
        _scheduleRepository = scheduleRepository;
        _historyRepository = historyRepository;
        _dateTimeProvider = dateTimeProvider;
        _scheduleRunningConditionChecker = scheduleRunningConditionChecker;
    }

    public Task AddToRunned(IEnumerable<Schedule> schedules)
    {
        return _historyRepository.AddRunnedSchedules(schedules.Select(s => s.Id), _dateTimeProvider.UtcNow);
    }

    public Task AddToRunned(Schedule schedule)
    {
        return _historyRepository.AddRunnedSchedules(new List<Guid>() { schedule.Id }, _dateTimeProvider.UtcNow);
    }

    public IEnumerable<Schedule> GetSchedulesToRun()
    {
        var numberOfBatch = 0;

        var todayRunnedSchedules = _historyRepository.GetTodayRunnedSchedules(_dateTimeProvider.UtcNow);
        var schedules = _scheduleRepository.GetSchedulesWithDevicesExcluding(todayRunnedSchedules);

        var schedulesToRun = new List<Schedule>();

        while (true)
        {
            var schedulesInMemory = schedules
                .OrderBy(s => s.Modified)
                .Skip(numberOfBatch * BatchSize)
                .Take(BatchSize)
                .ToList();

            var schedulesBatch = schedulesInMemory
                .Where(s => _scheduleRunningConditionChecker.CheckScheduleRunCondition(s.Hour, s.Minute))
                .ToList();

            if (!schedulesBatch.Any()) break;

            schedulesToRun.AddRange(schedulesBatch);
            numberOfBatch += 1;
        }

        return schedulesToRun;
    }
}
