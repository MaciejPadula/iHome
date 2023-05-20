using iHome.Microservices.Devices.Contract;
using iHome.Microservices.Devices.Contract.Models;

namespace iHome.Core.Services;

public interface IDevicesForSchedulingAccessor
{
    Task<IEnumerable<DeviceModel>> Get(string userId);
}

public class DevicesForSchedulingAccessor : IDevicesForSchedulingAccessor
{
    private readonly IDeviceManagementService _deviceManagementService;
    private readonly IDeviceDataService _deviceDataService;

    private readonly List<DeviceType> _devicesForScheduling = new()
    {
        DeviceType.RGBLamp,
        DeviceType.RobotVaccumCleaner
    };

    public DevicesForSchedulingAccessor(IDeviceManagementService deviceManagementService, IDeviceDataService deviceDataService)
    {
        _deviceManagementService = deviceManagementService;
        _deviceDataService = deviceDataService;
    }

    public async Task<IEnumerable<DeviceModel>> Get(string userId)
    {
        var devices = (await _deviceManagementService.GetDevices(new()
        {
            UserId = userId
        })).Devices
            .Where(d => _devicesForScheduling.Contains(d.Type))
            .ToList();

        foreach (var device in devices)
        {
            device.Data = (await _deviceDataService.GetDeviceData(new()
            {
                DeviceId = device.Id
            })).DeviceData;
        }

        return devices;
    }
}
