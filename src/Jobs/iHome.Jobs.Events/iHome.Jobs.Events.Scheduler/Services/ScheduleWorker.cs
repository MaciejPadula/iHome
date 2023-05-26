using iHome.Infrastructure.Queue.Models;
using iHome.Infrastructure.Queue.Service.Write;
using iHome.Jobs.Events.Contexts;
using iHome.Jobs.Events.Services;
using System.Threading;

namespace iHome.Jobs.Events.Scheduler.Services
{
    public class ScheduleWorker : BackgroundService
    {
        private readonly ILogger<ScheduleWorker> _logger;
        private readonly WorkerContext _context;
        private readonly ISchedulesProvider _schedulesProvider;
        private readonly IQueueWriter<DataUpdateModel> _queueWriter;
        private readonly PeriodicTimer _periodicTimer;

        public ScheduleWorker(ILogger<ScheduleWorker> logger, WorkerContext context, ISchedulesProvider schedulesProvider, IQueueWriter<DataUpdateModel> queueWriter)
        {
            _logger = logger;
            _context = context;
            _schedulesProvider = schedulesProvider;
            _queueWriter = queueWriter;

            _periodicTimer = new PeriodicTimer(_context.JobDelay);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (_context.IsRunning && await _periodicTimer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation("{Date}===STARTING PROCESS===", DateTime.UtcNow.ToLongTimeString());

                var schedulesProcessedCount = await Working();
                if (schedulesProcessedCount == 0) continue;

                _logger.LogInformation("Schedules Processed: {count}", schedulesProcessedCount);
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
                        DeviceId = device.DeviceId,
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
}