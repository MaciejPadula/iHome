using iHome.Infrastructure.Queue.DataUpdate.Write;
using iHome.Infrastructure.Queue.Models;
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
    private readonly IDataUpdateQueueWriter _queueWriter;
    private readonly PeriodicTimer _periodicTimer;

    public ScheduleWorker(ILogger<IScheduleWorker> logger, WorkerContext context, ISchedulesProvider schedulesProvider, IDataUpdateQueueWriter queueWriter)
    {
        _logger = logger;
        _context = context;
        _schedulesProvider = schedulesProvider;
        _queueWriter = queueWriter;

        _periodicTimer = new PeriodicTimer(_context.JobDelay);
    }

    public async Task Start()
    {
        while (_context.IsRunning && await _periodicTimer.WaitForNextTickAsync())
        {
            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()}===STARTING PROCESS===");

            var schedulesProcessedCount = await Working();
            if (schedulesProcessedCount == 0) continue;

            _logger.LogInformation($"Schedules Processed: {schedulesProcessedCount}");
        }
    }

    public async Task<int> Working()
    {
        var tasks = new List<Task>();
        var schedules = await _schedulesProvider.GetSchedulesToRun();
        var numberOfTasks = 0;

        foreach (var schedule in schedules)
        {
            foreach (var device in schedule.ScheduleDevices)
            {
                tasks.Add(_queueWriter.Push(new DataUpdateModel
                {
                    MacAddress = device.Device?.MacAddress ?? string.Empty,
                    DeviceData = device.DeviceData
                }));
            }
            ++numberOfTasks;
        }

        await Task.WhenAll(tasks);
        await _schedulesProvider.AddToRunned(schedules);

        return numberOfTasks;
    }
}
