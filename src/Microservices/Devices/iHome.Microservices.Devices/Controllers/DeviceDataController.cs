using iHome.Core.Repositories.Devices;
using iHome.Microservices.Devices.Contract;
using iHome.Microservices.Devices.Contract.Models.Request;
using iHome.Microservices.Devices.Contract.Models.Response;
using iHome.Microservices.Devices.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.Devices.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DeviceDataController : ControllerBase, IDeviceDataService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDeviceDataRepository _deviceDataRepository;

        public DeviceDataController(IDeviceRepository deviceRepository, IDeviceDataRepository deviceDataRepository)
        {
            _deviceRepository = deviceRepository;
            _deviceDataRepository = deviceDataRepository;
        }

        [HttpPost]
        public async Task<GetDeviceDataResponse> GetDeviceData(GetDeviceDataRequest request)
        {
            // validation

            var device = await _deviceRepository.GetByDeviceId(request.DeviceId) ?? throw new Exception();

            return new() 
            {
                DeviceData = await _deviceDataRepository.GetDeviceData(device.MacAddress)
            };
        }

        [HttpPost]
        public async Task SetDeviceData(SetDeviceDataRequest request)
        {
            // validation

            var device = await _deviceRepository.GetByDeviceId(request.DeviceId) ?? throw new Exception();

            await _deviceDataRepository.SetDeviceData(device.MacAddress, request.Data);
        }
    }
}
