using iHome.Core.Repositories.Devices;
using iHome.Microservices.Devices.Contract;
using iHome.Microservices.Devices.Contract.Models.Request;
using iHome.Microservices.Devices.Contract.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.Devices.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DeviceManagementController : ControllerBase, IDeviceManagementService
    {
        private readonly IDeviceRepository _deviceRepository;

        public DeviceManagementController(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        [HttpPost]
        public async Task<AddDeviceResponse> AddDevice(AddDeviceRequest request)
        {
            return new()
            {
                DeviceId = await _deviceRepository.Add(request.Name, request.MacAddress, request.Type, request.RoomId)
            };
        }

        [HttpPost]
        public Task ChangeDeviceRoom(ChangeDeviceRoomRequest request)
        {
            //validation

            return _deviceRepository.ChangeRoom(request.DeviceId, request.RoomId);
        }

        [HttpPost]
        public async Task<GetDeviceResponse> GetDevice(GetDeviceRequest request)
        {
            //validation

            return new()
            {
                Device = await _deviceRepository.GetByDeviceId(request.DeviceId)
            };
        }

        [HttpPost]
        public async Task<GetDevicesResponse> GetDevices(GetDevicesRequest request)
        {
            if (request.RoomId == default!)
            {
                return new()
                {
                    Devices = await _deviceRepository.GetByUserId(request.UserId)
                };
            }

            return new()
            {
                Devices = await _deviceRepository.GetByRoomId(request.RoomId)
            };
        }

        [HttpPost]
        public Task RemoveDevice(RemoveDeviceRequest request)
        {
            //validation

            return _deviceRepository.Remove(request.DeviceId);
        }

        [HttpPost]
        public Task RenameDevice(RenameDeviceRequest request)
        {
            //validation

            return _deviceRepository.Rename(request.DeviceId, request.NewName);
        }
    }
}
