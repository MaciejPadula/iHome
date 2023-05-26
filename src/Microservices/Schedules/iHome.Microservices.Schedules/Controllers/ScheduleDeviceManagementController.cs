using iHome.Microservices.Schedules.Contract;
using iHome.Microservices.Schedules.Contract.Models.Request;
using iHome.Microservices.Schedules.Contract.Models.Response;
using iHome.Microservices.Schedules.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.Schedules.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ScheduleDeviceManagementController : ControllerBase, IScheduleDeviceManagementService
    {
        private readonly IScheduleDeviceRepository _repository;

        public ScheduleDeviceManagementController(IScheduleDeviceRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task AddOrUpdateDeviceSchedule(AddOrUpdateDeviceScheduleRequest request)
        {
            //validation

            var existingDevice = await _repository.GetByIdAndScheduleId(request.DeviceId, request.ScheduleId);

            if (existingDevice == null)
            {
                await _repository.Add(request.ScheduleId, request.DeviceId, request.DeviceData);
                return;
            }

            await _repository.Update(request.ScheduleId, request.DeviceId, request.DeviceData);
        }

        [HttpPost]
        public async Task<GetDevicesInScheduleCountResponse> GetDevicesInScheduleCount(GetDevicesInScheduleCountRequest request)
        {
            return new()
            {
                NumberOfDevices = await _repository.CountByScheduleId(request.ScheduleId)
            };
        }

        [HttpPost]
        public async Task<GetScheduleDeviceResponse> GetScheduleDevice(GetScheduleDeviceRequest request)
        {
            //validation

            return new()
            {
                ScheduleDevice = await _repository.GetByIdAndScheduleId(request.DeviceId, request.ScheduleId)
            };
        }

        [HttpPost]
        public async Task<GetScheduleDevicesResponse> GetScheduleDevices(GetScheduleDevicesRequest request)
        {
            //validation

            return new()
            {
                ScheduleDevices = await _repository.GetByScheduleId(request.ScheduleId)
            };
        }

        [HttpPost]
        public Task RemoveDeviceSchedule(RemoveDeviceScheduleRequest request)
        {
            //validation

            return _repository.Remove(request.ScheduleId, request.DeviceId);
        }
    }
}
