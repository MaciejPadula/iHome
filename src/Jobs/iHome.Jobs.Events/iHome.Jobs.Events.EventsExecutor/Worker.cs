using iHome.Infrastructure.Queue.Models;
using iHome.Infrastructure.Queue.Service.Read;

namespace iHome.Jobs.Events.EventsExecutor
{
    public class Worker : BackgroundService
    {
        private readonly IQueueReader<DataUpdateModel> _queueReader;
        private readonly IDeviceDataService _deviceDataService;
        private readonly PeriodicTimer _timer;
        private readonly ILogger<Worker> _logger;

        public Worker(IQueueReader<DataUpdateModel> queueReader, IDeviceDataService deviceDataService, ILogger<Worker> logger)
        {
            _queueReader = queueReader;
            _deviceDataService = deviceDataService;
            _logger = logger;

            _timer = new PeriodicTimer(TimeSpan.FromMilliseconds(500));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested && await _timer.WaitForNextTickAsync())
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
            }
        }
    }
}