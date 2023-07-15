using iHome.Microservices.Devices.Contract.Models.Request;
using iHome.Microservices.Devices.Contract.Models.Response;

namespace iHome.Microservices.Devices.Handlers;

public interface IDeviceDataHandler
{
    Task SetDeviceData(SetDeviceDataRequest request);
    Task<GetDeviceDataResponse> GetDeviceData(GetDeviceDataRequest request);
}
