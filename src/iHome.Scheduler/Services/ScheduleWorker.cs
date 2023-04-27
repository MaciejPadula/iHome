using iHome.Scheduler.Contexts;
using Microsoft.Extensions.Logging;

namespace iHome.Scheduler.Services;

public interface IScheduleWorker
{
    Task Start();
}

public class ScheduleWorker : IScheduleWorker
{
    private readonly ILogger<IScheduleWorker> _logger;
    private readonly WorkerContext _context;
    private readonly ISchedulesProvider _schedulesProvider;
    private readonly ISchedulesUpdater _schedulesUpdater;

    public ScheduleWorker(ILogger<IScheduleWorker> logger, WorkerContext context, ISchedulesProvider schedulesProvider, ISchedulesUpdater schedulesUpdater)
    {
        _logger = logger;
        _context = context;
        _schedulesProvider = schedulesProvider;
        _schedulesUpdater = schedulesUpdater;
    }

    public async Task Start()
    {
        while (_context.IsRunning)
        {
            _logger.LogInformation("===STARTING PROCESS===");
            var schedulesProcessedCount = await Working();
            _logger.LogInformation($"Schedules Processed: {schedulesProcessedCount}");

            await Task.Delay(_context.JobDelay);
        }
    }

    public async Task<int> Working()
    {
        var runnedSchedules = 0;
        var tasks = new List<Task>();
        var schedules = await _schedulesProvider.GetSchedulesToRun();

        foreach (var schedule in schedules)
        {
            tasks.Add(_schedulesUpdater.UpdateDevicesFromSchedule(schedule));
        }

        await Task.WhenAll(tasks);
        await _schedulesProvider.AddToRunned(schedules);

        return tasks.Count;
    }
}
