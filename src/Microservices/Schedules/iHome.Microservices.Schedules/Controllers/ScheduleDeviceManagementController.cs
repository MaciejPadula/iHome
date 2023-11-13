using iHome.Microservices.Schedules.Contract;
using iHome.Microservices.Schedules.Contract.Models.Request;
using iHome.Microservices.Schedules.Contract.Models.Response;
using iHome.Microservices.Schedules.Infrastructure.Repositories;
using iHome.Microservices.Schedules.Providers;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.Schedules.Controllers
{
    public class ScheduleDeviceManagementController : ControllerBase, IScheduleDeviceManagementService
    {
        private readonly IScheduleDeviceRepository _repository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDeviceForSchedulingTypesProvider _typesProvider;

        public ScheduleDeviceManagementController(IScheduleDeviceRepository repository, IDeviceRepository deviceRepository, IDeviceForSchedulingTypesProvider typesProvider)
        {
            _repository = repository;
            _deviceRepository = deviceRepository;
            _typesProvider = typesProvider;
        }

        [HttpPost]
        public async Task<AddOrUpdateDeviceScheduleResponse> AddOrUpdateDeviceSchedule([FromBody] AddOrUpdateDeviceScheduleRequest request)
        {
            var existingDevice = await _repository.GetByIdAndScheduleId(request.DeviceId, request.ScheduleId);
            Guid? id;

            if (existingDevice == null)
            {
                id = await _repository.Add(request.ScheduleId, request.DeviceId, request.DeviceData);
            }
            else
            {
                await _repository.Update(request.ScheduleId, request.DeviceId, request.DeviceData);
                id = existingDevice.Id;
            }

            return new()
            {
                ScheduleDeviceId = id.Value
            };
        }

        public async Task<GetDevicesForSchedulingResponse> GetDevicesForScheduling([FromBody] GetDevicesForSchedulingRequest request)
        {
            return new()
            {
                DeviceIds = await _deviceRepository.GetDevicesForUserByTypes(request.UserId, _typesProvider.Provide())
            };
        }

        [HttpPost]
        public async Task<GetDevicesInScheduleCountResponse> GetDevicesInScheduleCount([FromBody] GetDevicesInScheduleCountRequest request)
        {
            return new()
            {
                NumberOfDevices = await _repository.CountByScheduleId(request.ScheduleId)
            };
        }

        [HttpPost]
        public async Task<GetScheduleDeviceResponse> GetScheduleDevice([FromBody] GetScheduleDeviceRequest request)
        {
            return new()
            {
                ScheduleDevice = await _repository.GetByIdAndScheduleId(request.DeviceId, request.ScheduleId)
            };
        }

        [HttpPost]
        public async Task<GetScheduleDevicesResponse> GetScheduleDevices([FromBody] GetScheduleDevicesRequest request)
        {
            return new()
            {
                ScheduleDevices = await _repository.GetByScheduleId(request.ScheduleId)
            };
        }

        [HttpPost]
        public Task RemoveDeviceSchedule([FromBody] RemoveDeviceScheduleRequest request)
        {
            return _repository.Remove(request.ScheduleId, request.DeviceId);
        }
    }
}
