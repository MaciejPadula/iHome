using iHome.Infrastructure.Queue.Models;
using iHome.Infrastructure.Queue.Service.Read;
using iHome.Microservices.Devices.Contract;

namespace iHome.EventsExecutor;

public class Worker
{
    private readonly IQueueReader<DataUpdateModel> _queueReader;
    private readonly IDeviceDataService _deviceDataService;
    private readonly PeriodicTimer _timer;

    public Worker(IQueueReader<DataUpdateModel> queueReader, IDeviceDataService deviceDataService)
    {
        _queueReader = queueReader;
        _deviceDataService = deviceDataService;

        _timer = new PeriodicTimer(TimeSpan.FromMilliseconds(500));
    }

    public async Task Start()
    {
        while (await _timer.WaitForNextTickAsync())
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
