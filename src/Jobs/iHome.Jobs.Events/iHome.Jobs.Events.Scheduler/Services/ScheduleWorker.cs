using iHome.Infrastructure.Queue.Models;
using iHome.Infrastructure.Queue.Service.Read;
using iHome.Infrastructure.Queue.Service.Write;
using iHome.Jobs.Events.Infrastructure.Models;
using iHome.Jobs.Events.Services;
using Microsoft.ApplicationInsights;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace iHome.Jobs.Events.Scheduler.Services
{
    public class ScheduleWorker : BackgroundService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly TelemetryClient _telemetryClient;

        private ISchedulesProvider _schedulesProvider = default!;
        private IQueueWriter<DataUpdateModel> _queueWriter = default!;

        private const int ChunkSize = 1000;

        public ScheduleWorker(IHostApplicationLifetime hostApplicationLifetime, TelemetryClient telemetryClient, IServiceScopeFactory serviceScopeFactory)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _telemetryClient = telemetryClient;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void InitializeScope()
        {
            var scope = _serviceScopeFactory.CreateScope();
            _schedulesProvider = scope.ServiceProvider.GetRequiredService<ISchedulesProvider>();
            _queueWriter = scope.ServiceProvider.GetRequiredService<IQueueWriter<DataUpdateModel>>();


            if (_schedulesProvider is null || _queueWriter is null)
            {
                throw new ArgumentNullException();
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                InitializeScope();
                var schedulesProcessedCount = await Working();

                if (schedulesProcessedCount != 0)
                {
                    _telemetryClient.TrackEvent(typeof(ScheduleWorker).FullName, new Dictionary<string, string> { { "SchedulesProcessed", schedulesProcessedCount.ToString() } });
                }
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
            }
            finally
            {
                _hostApplicationLifetime.StopApplication();
            }
        }

        public async Task<int> Working()
        {
            var tasks = new List<Task>();
            var schedules = _schedulesProvider.GetSchedulesToRun().ToList();

            foreach (var schedule in schedules)
            {
                tasks.Add(ProcessSchedule(schedule));
            }

            foreach (var tasksChunk in tasks.Chunk(ChunkSize))
            {
                await Task.WhenAll(tasksChunk);
            }

            return schedules.Count;
        }

        private async Task ProcessSchedule(Schedule schedule)
        {
            foreach (var device in schedule.ScheduleDevices)
            {
                await _queueWriter.Push(new DataUpdateModel
                {
                    DeviceId = device.DeviceId,
                    DeviceData = device.DeviceData
                });
            }

            await _schedulesProvider.AddToRunned(schedule);
        }
    }
}