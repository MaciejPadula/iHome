using iHome.Infrastructure.Queue.Models;
using iHome.Infrastructure.Queue.Service.Read;
using iHome.Microservices.Devices.Contract;

namespace iHome.Jobs.Events.EventsExecutor
{
    public class Worker : BackgroundService
    {
        private readonly IQueueReader<DataUpdateModel> _queueReader;
        private readonly IDeviceDataService _deviceDataService;
        private readonly ILogger<Worker> _logger;

        public Worker(IQueueReader<DataUpdateModel> queueReader, IDeviceDataService deviceDataService, ILogger<Worker> logger)
        {
            _queueReader = queueReader;
            _deviceDataService = deviceDataService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PROCESS STARTED");
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
            _logger.LogInformation("EVENTS EXECUTED: {events}", tasks.Count);
        }
    }
}