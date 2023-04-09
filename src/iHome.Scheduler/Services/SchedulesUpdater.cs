using iHome.Scheduler.Infrastructure.Models;
using iHome.Scheduler.Infrastructure.Services;
using iHome.Scheduler.Repositories.Models;

namespace iHome.Scheduler.Services;

public interface ISchedulesUpdater
{
    Task UpdateDevicesFromSchedule(Schedule schedule);
}

public class SchedulesUpdater : ISchedulesUpdater
{
    private readonly IDeviceDataService _deviceDataService;

    public SchedulesUpdater(IDeviceDataService deviceDataService)
    {
        _deviceDataService = deviceDataService;
    }

    public async Task UpdateDevicesFromSchedule(Schedule schedule)
    {
        var tasks = new List<Task>();

        foreach(var device in schedule.ScheduleDevices)
        {
            tasks.Add(_deviceDataService.UpdateDeviceData(new UpdateDeviceDataRequest
            {
                DeviceId = device.Id,
                DeviceData = device.DeviceData
            }));
        }

        await Task.WhenAll(tasks);
    }
}
