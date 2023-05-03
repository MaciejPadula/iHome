using iHome.Infrastructure.Firebase.Repositories;
using iHome.Infrastructure.Queue.DataUpdate.Read;

namespace iHome.EventsExecutor
{
    public class Worker
    {
        private readonly IDataUpdateQueueReader _queueReader;
        private readonly IDeviceDataRepository _deviceDataRepository;
        private readonly PeriodicTimer _timer;

        public Worker(IDataUpdateQueueReader queueReader, IDeviceDataRepository deviceDataRepository)
        {
            _queueReader = queueReader;
            _deviceDataRepository = deviceDataRepository;

            _timer = new PeriodicTimer(TimeSpan.FromMilliseconds(500));
        }

        public async Task Start()
        {
            while(await _timer.WaitForNextTickAsync())
            {
                var tasks = new List<Task>();

                while (await _queueReader.Peek() != null)
                {
                    var device = await _queueReader.Pop();
                    if (device == null) continue;

                    tasks.Add(_deviceDataRepository.SetData(device.MacAddress.ToString(), device.DeviceData));
                }

                await Task.WhenAll(tasks);
            }
        }
    }
}
