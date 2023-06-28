using iHome.Infrastructure.Queue.Models;
using iHome.Infrastructure.Queue.Service.Read;
using iHome.Microservices.Devices.Contract;
using Microsoft.ApplicationInsights;

namespace iHome.Jobs.Events.EventsExecutor
{
    public class Worker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        private readonly TelemetryClient _telemetryClient;
        private readonly PeriodicTimer _timer;
        private readonly TimeSpan Delay = TimeSpan.FromSeconds(5);

        private IQueueReader<DataUpdateModel>? _queueReader;
        private IDeviceDataService? _deviceDataService;

        public Worker(IHostApplicationLifetime hostApplicationLifetime, IServiceScopeFactory serviceScopeFactory, TelemetryClient telemetryClient)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _serviceScopeFactory = serviceScopeFactory;
            _telemetryClient = telemetryClient;

            _timer = new PeriodicTimer(Delay);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                (_deviceDataService, _queueReader) = InitializeScope();
                while (await _timer.WaitForNextTickAsync(stoppingToken))
                {
                    var eventsCount = await Working();

                    if (eventsCount == 0) continue;
                    _telemetryClient.TrackEvent("Results", new Dictionary<string, string> { { "Events executed", eventsCount.ToString() } });
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

        public (IDeviceDataService, IQueueReader<DataUpdateModel>) InitializeScope()
        {
            var scope = _serviceScopeFactory.CreateScope();
            return (
                scope.ServiceProvider.GetRequiredService<IDeviceDataService>(),
                scope.ServiceProvider.GetRequiredService<IQueueReader<DataUpdateModel>>()
            );
        }

        public async Task<int> Working()
        {
            if (_queueReader is null || _deviceDataService is null) return 0;
            var tasks = new List<Task>();

            while (await _queueReader.Peek() is not null)
            {
                var device = await _queueReader.Pop();
                if (device is null) continue;

                tasks.Add(_deviceDataService.SetDeviceData(new()
                {
                    DeviceId = device.DeviceId,
                    Data = device.DeviceData
                }));
            }

            await Task.WhenAll(tasks);
            return tasks.Count;
        }
    }
}