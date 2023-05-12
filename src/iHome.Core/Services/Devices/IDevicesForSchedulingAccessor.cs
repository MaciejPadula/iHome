using iHome.Core.Models;

namespace iHome.Core.Services.Devices;

public interface IDevicesForSchedulingAccessor
{
    Task<IEnumerable<DeviceModel>> Get(string userId);
}
