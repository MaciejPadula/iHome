using iHome.Infrastructure.Queue.Models;
using iHome.Infrastructure.Queue.Service.Write;
using iHome.Jobs.Events.Services;

namespace iHome.Jobs.Events.Scheduler.Services
{
    public class ScheduleWorker : BackgroundService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ILogger<ScheduleWorker> _logger;
        private readonly ISchedulesProvider _schedulesProvider;
        private readonly IQueueWriter<DataUpdateModel> _queueWriter;

        public ScheduleWorker(ILogger<ScheduleWorker> logger, ISchedulesProvider schedulesProvider, IQueueWriter<DataUpdateModel> queueWriter, IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            _schedulesProvider = schedulesProvider;
            _queueWriter = queueWriter;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("===STARTING PROCESS===");

                var schedulesProcessedCount = await Working();

                _logger.LogInformation("Schedules Processed: {count}", schedulesProcessedCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(ex));
            }
            finally
            {
                _hostApplicationLifetime.StopApplication();
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