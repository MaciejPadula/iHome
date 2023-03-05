using iHome.Devices.Contract.Models.Requests;

namespace iHome.Devices.Contract.Interfaces;

public interface IDeviceDataService
{
    string GetDeviceData(GetDeviceDataRequest request);
    void SetDeviceData(SetDeviceDataRequest request);
}
