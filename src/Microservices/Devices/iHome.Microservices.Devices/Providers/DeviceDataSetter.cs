using iHome.Microservices.Devices.Contract.Models;
using iHome.Microservices.Devices.Handlers;

namespace iHome.Microservices.Devices.Providers;

public class DeviceDataSetter : IDeviceDataSetter
{
    private readonly IDeviceDataHandler _deviceDataHandler;

    public DeviceDataSetter(IDeviceDataHandler deviceDataHandler)
    {
        _deviceDataHandler = deviceDataHandler;
    }

    public async Task<DeviceModel?> Set(DeviceModel? device)
    {
        if (device is null) return null;

        var response = await _deviceDataHandler.GetDeviceData(new() { DeviceId = device.Id });
        device.Data = response.DeviceData;
        return device;
    }

    public async Task<List<DeviceModel>> Set(List<DeviceModel> devices)
    {
        foreach (var device in devices)
        {
            await Set(device);
        }

        return devices;
    }
}
