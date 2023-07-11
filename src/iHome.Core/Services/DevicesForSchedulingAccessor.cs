using iHome.Microservices.Devices.Contract;
using iHome.Microservices.Devices.Contract.Models;
using iHome.Microservices.Schedules.Contract;

namespace iHome.Core.Services;

public interface IDevicesForSchedulingAccessor
{
    Task<IEnumerable<DeviceModel>> Get(string userId);
}

public class DevicesForSchedulingAccessor : IDevicesForSchedulingAccessor
{
    private readonly IDeviceManagementService _deviceManagementService;
    private readonly IScheduleDeviceManagementService _scheduleDeviceManagementService;
    private readonly IDeviceDataService _deviceDataService;

    public DevicesForSchedulingAccessor(IDeviceManagementService deviceManagementService,
        IDeviceDataService deviceDataService,
        IScheduleDeviceManagementService scheduleDeviceManagementService)
    {
        _deviceManagementService = deviceManagementService;
        _deviceDataService = deviceDataService;
        _scheduleDeviceManagementService = scheduleDeviceManagementService;
    }

    public async Task<IEnumerable<DeviceModel>> Get(string userId)
    {
        var deviceIdsForScheduling = await _scheduleDeviceManagementService.GetDevicesForScheduling(new()
        {
            UserId = userId
        });

        if(deviceIdsForScheduling is null)
        {
            return Enumerable.Empty<DeviceModel>();
        }

        var devices = await _deviceManagementService.GetDevicesByIds(new()
        {
            UserId = userId,
            DeviceIds = deviceIdsForScheduling.DeviceIds
        });

        if (devices?.Devices is null)
        {
            return Enumerable.Empty<DeviceModel>();
        }

        foreach (var device in devices.Devices)
        {
            device.Data = (await _deviceDataService.GetDeviceData(new()
            {
                DeviceId = device.Id
            })).DeviceData;
        }

        return devices.Devices;
    }
}
