using iHome.Infrastructure.Queue.Models;
using iHome.Infrastructure.Queue.Service.Write;
using iHome.Jobs.Events.Services;
using Microsoft.ApplicationInsights;
using Microsoft.IdentityModel.Abstractions;

namespace iHome.Jobs.Events.Scheduler.Services
{
    public class ScheduleWorker : BackgroundService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ISchedulesProvider _schedulesProvider;
        private readonly IQueueWriter<DataUpdateModel> _queueWriter;
        private readonly TelemetryClient _telemetryClient;

        public ScheduleWorker(ISchedulesProvider schedulesProvider, IQueueWriter<DataUpdateModel> queueWriter, IHostApplicationLifetime hostApplicationLifetime, TelemetryClient telemetryClient)
        {
            _schedulesProvider = schedulesProvider;
            _queueWriter = queueWriter;
            _hostApplicationLifetime = hostApplicationLifetime;
            _telemetryClient = telemetryClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var schedulesProcessedCount = await Working();

                if(schedulesProcessedCount != 0)
                {
                    _telemetryClient.TrackEvent("Results", new Dictionary<string, string> { { "SchedulesProcessed", schedulesProcessedCount.ToString() } });
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
            var schedules = (await _schedulesProvider.GetSchedulesToRun()).ToList();

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
            }

            await Task.WhenAll(tasks);
            await _schedulesProvider.AddToRunned(schedules);

            return schedules.Count;
        }
    }
}