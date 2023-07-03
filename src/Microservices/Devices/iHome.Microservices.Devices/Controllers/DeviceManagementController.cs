using iHome.Core.Repositories.Devices;
using iHome.Microservices.Devices.Contract;
using iHome.Microservices.Devices.Contract.Models;
using iHome.Microservices.Devices.Contract.Models.Request;
using iHome.Microservices.Devices.Contract.Models.Response;
using iHome.Microservices.Devices.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.Devices.Controllers
{
    public class DeviceManagementController : ControllerBase, IDeviceManagementService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDeviceDataRepository _deviceDataRepository;

        public DeviceManagementController(IDeviceRepository deviceRepository, IDeviceDataRepository deviceDataRepository)
        {
            _deviceRepository = deviceRepository;
            _deviceDataRepository = deviceDataRepository;
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
        public async Task<GetDeviceResponse> GetDevice([FromBody] GetDeviceRequest request)
        {
            return new()
            {
                Device = await _deviceRepository.GetByDeviceId(request.DeviceId)
            };
        }

        [HttpPost]
        public async Task<GetDevicesResponse> GetDevices([FromBody] GetDevicesRequest request)
        {
            List<DeviceModel> devices;
            if (request.RoomId == default!)
            {
                devices = (await _deviceRepository.GetByUserId(request.UserId)).ToList();
            }
            else
            {
                devices = (await _deviceRepository.GetByRoomId(request.RoomId)).ToList();
            }

            devices = devices.ToList();

            foreach(var d in devices)
            {
                d.Data = await _deviceDataRepository.GetDeviceData(d.MacAddress);
            }

            return new()
            {
                Devices = devices
            };
        }

        public async Task<GetDevicesResponse> GetDevicesByIds([FromBody] GetDevicesByIdsRequest request)
        {
            return new() 
            {
                Devices = await _deviceRepository.GetByUserIdAndDeviceIds(request.UserId, request.DeviceIds)
            };
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
