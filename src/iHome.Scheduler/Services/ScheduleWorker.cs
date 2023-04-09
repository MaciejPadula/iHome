using iHome.Scheduler.Contexts;

namespace iHome.Scheduler.Services;

public interface IScheduleWorker
{
    Task Start();
}

public class ScheduleWorker : IScheduleWorker
{
    private readonly WorkerContext _context;
    private readonly ISchedulesProvider _schedulesProvider;
    private readonly ISchedulesUpdater _schedulesUpdater;

    public ScheduleWorker(WorkerContext context, ISchedulesProvider schedulesProvider, ISchedulesUpdater schedulesUpdater)
    {
        _context = context;
        _schedulesProvider = schedulesProvider;
        _schedulesUpdater = schedulesUpdater;
    }

    public async Task Start()
    {
        while (_context.IsRunning)
        {
            var tasks = new List<Task>();
            var schedules = await _schedulesProvider.GetSchedulesToRun(DateTime.UtcNow);
            
            foreach(var schedule in schedules )
            {
                tasks.Add(_schedulesUpdater.UpdateDevicesFromSchedule(schedule));
            }

            await Task.WhenAll(tasks);
            await Task.Delay(100);
        }
    }
}
