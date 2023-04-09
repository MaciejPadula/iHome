using iHome.Devices.Contract.Models.Requests;

namespace iHome.Devices.Contract.Interfaces;

public interface IDeviceDataService
{
    Task<string> GetDeviceData(GetDeviceDataRequest request);
    Task SetDeviceData(SetDeviceDataRequest request);
}
