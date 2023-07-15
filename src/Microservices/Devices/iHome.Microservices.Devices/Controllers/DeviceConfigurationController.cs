using iHome.Microservices.Devices.Contract;
using iHome.Microservices.Devices.Contract.Models.Request;
using iHome.Microservices.Devices.Contract.Models.Response;
using iHome.Microservices.Devices.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.Devices.Controllers
{
    public class DeviceConfigurationController : ControllerBase, IDeviceConfigurationService
    {
        private readonly IDeviceConfigurationRepository _deviceConfigurationRepositor;

        public DeviceConfigurationController(IDeviceConfigurationRepository deviceConfigurationRepositor)
        {
            _deviceConfigurationRepositor = deviceConfigurationRepositor;
        }

        [HttpPost]
        public async Task<GetDevicesToConfigureResponse> GetDevicesToConfigure([FromBody] GetDevicesToConfigureRequest request)
        {
            return new()
            {
                Devices = await _deviceConfigurationRepositor.GetDeviceToConfigure(request.Address)
            };
        }

        [HttpPost]
        public async Task RemoveDeviceToConfigure([FromBody] RemoveDeviceToConfigureRequest request)
        {
            await _deviceConfigurationRepositor.RemoveDeviceToConfigure(request.Address, request.DeviceToConfigureId);
        }
    }
}
