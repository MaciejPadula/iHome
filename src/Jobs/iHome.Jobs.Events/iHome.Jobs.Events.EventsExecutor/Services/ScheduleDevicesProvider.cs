using iHome.Infrastructure.Queue.Models;
using iHome.Infrastructure.Queue.Service.Read;

namespace iHome.Jobs.Events.EventsExecutor.Services;

public interface IScheduleDevicesProvider
{
    Task<IEnumerable<DataUpdateModel>> Provide();
}

public class ScheduleDevicesProvider : IScheduleDevicesProvider
{
    private readonly IQueueReader<DataUpdateModel> _queueReader;

    public ScheduleDevicesProvider(IQueueReader<DataUpdateModel> queueReader)
    {
        _queueReader = queueReader;
    }

    public async Task<IEnumerable<DataUpdateModel>> Provide()
    {
        var data = new List<DataUpdateModel>();

        while (await _queueReader.Peek() is not null)
        {
            var device = await _queueReader.Pop();
            if (device is null) continue;

            data.Add(device);
        }

        return data;
    }
}
