using iHome.Microservices.Devices.Contract.Models;

namespace iHome.Microservices.Devices.Providers;

public interface IDeviceDataSetter
{
    Task<DeviceModel?> Set(DeviceModel? device);
    Task<List<DeviceModel>> Set(List<DeviceModel> devices);
}
