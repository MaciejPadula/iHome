using iHome.Devices.Contract.Models;

namespace iHome.Devices.Contract.Interfaces;

public interface IDeviceProvider
{
    string GetDeviceData(Guid deviceId);
    IEnumerable<Device> GetDevices(Guid roomId);
}
