using iHome.Devices.Contract.Models;
using iHome.Devices.Contract.Models.Requests;

namespace iHome.Devices.Contract.Interfaces;

public interface IDeviceProvider
{
    string GetDeviceData(GetDeviceDataRequest request);
    IEnumerable<Device> GetDevices(GetDevicesRequest request);
}
