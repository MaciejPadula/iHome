using iHome.Microservices.Devices.Contract.Models.Request;
using iHome.Microservices.Devices.Contract.Models.Response;

namespace iHome.Microservices.Devices.Managers;

public interface IDeviceManager
{
    Task<GetDevicesResponse> GetDevices(GetDevicesRequest request);
    Task<GetDeviceResponse> GetDevice(GetDeviceRequest request);
    Task<GetDevicesResponse> GetDevicesByIds(GetDevicesByIdsRequest request);
}
