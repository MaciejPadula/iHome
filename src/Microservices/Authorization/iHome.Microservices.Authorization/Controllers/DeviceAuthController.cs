using iHome.Microservices.Authorization.Contract;
using iHome.Microservices.Authorization.Contract.Models.Request;
using iHome.Microservices.Authorization.Contract.Models.Response;
using iHome.Microservices.Authorization.Managers;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.Authorization.Controllers
{
    public class DeviceAuthController : ControllerBase, IDeviceAuthService
    {
        private readonly IDeviceManager _deviceManager;

        public DeviceAuthController(IDeviceManager deviceManager)
        {
            _deviceManager = deviceManager;
        }

        [HttpPost]
        public async Task<AuthResponse> CanReadDevice([FromBody] DeviceAuthRequest request)
        {
            return new()
            {
                HasAccess = await _deviceManager.CanRead(request)
            };
        }

        [HttpPost]
        public async Task<AuthResponse> CanWriteDevice([FromBody] DeviceAuthRequest request)
        {
            return new()
            {
                HasAccess = await _deviceManager.CanWrite(request)
            };
        }
    }
}
