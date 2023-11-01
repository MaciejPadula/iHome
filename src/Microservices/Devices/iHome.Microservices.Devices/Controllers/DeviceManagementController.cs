using iHome.Core.Repositories.Devices;
using iHome.Microservices.Devices.Contract;
using iHome.Microservices.Devices.Contract.Models;
using iHome.Microservices.Devices.Contract.Models.Request;
using iHome.Microservices.Devices.Contract.Models.Response;
using iHome.Microservices.Devices.Handlers;
using iHome.Microservices.Devices.Infrastructure.Repositories;
using iHome.Microservices.Devices.Managers;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.Devices.Controllers
{
    public class DeviceManagementController : ControllerBase, IDeviceManagementService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDeviceManager _deviceManager;

        public DeviceManagementController(IDeviceRepository deviceRepository, IDeviceManager deviceManager)
        {
            _deviceRepository = deviceRepository;
            _deviceManager = deviceManager;
        }

        [HttpPost]
        public async Task<AddDeviceResponse> AddDevice([FromBody] AddDeviceRequest request)
        {
            return new()
            {
                DeviceId = await _deviceRepository.Add(request.Name, request.MacAddress, request.Type, request.RoomId)
            };
        }

        [HttpPost]
        public Task ChangeDeviceRoom([FromBody] ChangeDeviceRoomRequest request)
        {
            return _deviceRepository.ChangeRoom(request.DeviceId, request.RoomId);
        }

        [HttpPost]
        public Task<GetDeviceResponse> GetDevice([FromBody] GetDeviceRequest request)
        {
            return GetDevice(request);
        }

        [HttpPost]
        public Task<GetDevicesResponse> GetDevices([FromBody] GetDevicesRequest request)
        {
            return _deviceManager.GetDevices(request);
        }

        public Task<GetDevicesResponse> GetDevicesByIds([FromBody] GetDevicesByIdsRequest request)
        {
            return _deviceManager.GetDevicesByIds(request);
        }

        [HttpPost]
        public Task RemoveDevice([FromBody] RemoveDeviceRequest request)
        {
            return _deviceRepository.Remove(request.DeviceId);
        }

        [HttpPost]
        public Task RenameDevice([FromBody] RenameDeviceRequest request)
        {
            return _deviceRepository.Rename(request.DeviceId, request.NewName);
        }
    }
}
