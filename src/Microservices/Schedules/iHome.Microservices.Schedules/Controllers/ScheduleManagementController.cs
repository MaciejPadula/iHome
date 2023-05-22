using iHome.Microservices.Schedules.Contract;
using iHome.Microservices.Schedules.Contract.Models.Request;
using iHome.Microservices.Schedules.Contract.Models.Response;
using iHome.Microservices.Schedules.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.Schedules.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ScheduleManagementController : ControllerBase, IScheduleManagementService
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleManagementController(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        [HttpPost]
        public Task AddSchedule(AddScheduleRequest request)
        {
            return _scheduleRepository.Add(request.ScheduleName, request.Hour, request.Minute, request.UserId);
        }

        [HttpPost]
        public async Task<GetScheduleResponse> GetSchedule(GetScheduleRequest request)
        {
            //validation

            return new()
            {
                Schedule = await _scheduleRepository.GetById(request.ScheduleId)
            };
        }

        [HttpPost]
        public async Task<GetSchedulesResponse> GetSchedules(GetSchedulesRequest request)
        {
            return new()
            {
                Schedules = await _scheduleRepository.GetByUserId(request.UserId)
            };
        }

        [HttpPost]
        public Task RemoveSchedule(RemoveScheduleRequest request)
        {
            //validation

            return _scheduleRepository.Remove(request.ScheduleId);
        }

        [HttpPost]
        public Task UpdateScheduleTime(UpdateScheduleTimeRequest request)
        {
            //validation

            return _scheduleRepository.UpdateTime(request.ScheduleId, request.Hour, request.Minute);
        }
    }
}
