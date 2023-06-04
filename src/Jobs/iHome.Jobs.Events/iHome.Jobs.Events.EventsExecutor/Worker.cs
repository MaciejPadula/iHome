using iHome.Infrastructure.Queue.Models;
using iHome.Infrastructure.Queue.Service;
using iHome.Infrastructure.Queue.Service.Read;
using iHome.Microservices.Devices.Contract;
using Microsoft.Extensions.Hosting;

namespace iHome.Jobs.Events.EventsExecutor
{
    public class Worker : BackgroundService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IQueueReader<DataUpdateModel> _queueReader;
        private readonly IDeviceDataService _deviceDataService;
        private readonly ILogger<Worker> _logger;
        private readonly PeriodicTimer _timer;
        private readonly TimeSpan Delay = TimeSpan.FromSeconds(5);

        public Worker(IQueueReader<DataUpdateModel> queueReader, IDeviceDataService deviceDataService, ILogger<Worker> logger, IHostApplicationLifetime hostApplicationLifetime)
        {
            _queueReader = queueReader;
            _deviceDataService = deviceDataService;
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;

            _timer = new PeriodicTimer(Delay);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while(await _timer.WaitForNextTickAsync(stoppingToken))
                {
                    _logger.LogInformation("PROCESS STARTED");

                    var eventsCount = await Working();

                    _logger.LogInformation("EVENTS EXECUTED: {events}", eventsCount);
                }
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

            while (await _queueReader.Peek() != null)
            {
                var device = await _queueReader.Pop();
                if (device == null) continue;

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