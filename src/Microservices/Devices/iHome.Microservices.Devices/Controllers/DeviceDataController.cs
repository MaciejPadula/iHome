using iHome.Microservices.Devices.Contract;
using iHome.Microservices.Devices.Contract.Models.Request;
using iHome.Microservices.Devices.Contract.Models.Response;
using iHome.Microservices.Devices.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.Devices.Controllers
{
    public class DeviceDataController : ControllerBase, IDeviceDataService
    {
        private readonly IDeviceDataHandler _deviceDataHandler;

        public DeviceDataController(IDeviceDataHandler deviceDataHandler)
        {
            _deviceDataHandler = deviceDataHandler;
        }

        [HttpPost]
        public async Task<GetDeviceDataResponse> GetDeviceData([FromBody] GetDeviceDataRequest request)
        {
            return await _deviceDataHandler.GetDeviceData(request);
        }

        [HttpPost]
        public async Task SetDeviceData([FromBody] SetDeviceDataRequest request)
        {
            await _deviceDataHandler.SetDeviceData(request);
        }
    }
}
