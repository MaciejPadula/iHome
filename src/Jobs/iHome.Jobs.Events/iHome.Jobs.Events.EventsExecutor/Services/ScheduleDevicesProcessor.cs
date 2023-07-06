using iHome.Infrastructure.Queue.Models;
using iHome.Microservices.Devices.Contract;

namespace iHome.Jobs.Events.EventsExecutor.Services;

public interface IScheduleDevicesProcessor
{
    Task<int> Process(IEnumerable<DataUpdateModel> dataUpdates);
}

public class ScheduleDevicesProcessor : IScheduleDevicesProcessor
{
    private readonly IDeviceDataService _deviceDataService;

    public ScheduleDevicesProcessor(IDeviceDataService deviceDataService)
    {
        _deviceDataService = deviceDataService;
    }

    public async Task<int> Process(IEnumerable<DataUpdateModel> dataUpdates)
    {
        var count = 0;
        foreach (var device in dataUpdates)
        {
            await _deviceDataService.SetDeviceData(new()
            {
                DeviceId = device.DeviceId,
                Data = device.DeviceData
            });
            count += 1;
        }

        return count;
    }
}
