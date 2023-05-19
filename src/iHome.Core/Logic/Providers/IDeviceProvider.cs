using iHome.Core.Models;

namespace iHome.Core.Logic.Providers;

public interface IDeviceProvider
{
    Task<DeviceModel?> Get(Guid deviceId);
}
