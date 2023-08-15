using iHome.Microservices.Schedules.Contract;
using iHome.Microservices.Schedules.Contract.Models.Request;
using iHome.Microservices.Schedules.Contract.Models.Response;
using iHome.Microservices.Schedules.Infrastructure.Repositories;
using iHome.Microservices.Schedules.Logic.Helpers;
using iHome.Microservices.Schedules.Managers;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.Schedules.Controllers
{
    public class ScheduleManagementController : ControllerBase, IScheduleManagementService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IScheduleRunHistoryRepository _scheduleRunHistoryRepository;
        private readonly IScheduleManager _scheduleManager;

        public ScheduleManagementController(IScheduleRepository scheduleRepository, IScheduleRunHistoryRepository schedulesRunHistoryRepository, IScheduleManager scheduleManager)
        {
            _scheduleRepository = scheduleRepository;
            _scheduleRunHistoryRepository = schedulesRunHistoryRepository;
            _scheduleManager = scheduleManager;
        }

        [HttpPost]
        public Task AddSchedule([FromBody] AddScheduleRequest request)
        {
            return _scheduleRepository.Add(request.ScheduleName, request.Hour, request.Minute, request.UserId);
        }

        [HttpPost]
        public async Task<GetScheduleResponse> GetSchedule([FromBody] GetScheduleRequest request)
        {
            var schedule = await _scheduleRepository.GetById(request.ScheduleId);

            schedule.Runned = await _scheduleRunHistoryRepository.ScheduleRunned(request.ScheduleId, DateTime.UtcNow.StartOfDay());

            return new()
            {
                Schedule = schedule
            };
        }

        [HttpPost]
        public Task<GetSchedulesResponse> GetSchedules([FromBody] GetSchedulesRequest request)
        {
            return _scheduleManager.GetSchedules(request);
        }

        [HttpPost]
        public Task<GetSchedulesResponse> GetSchedulesWithDevices([FromBody] GetSchedulesWithDevicesRequest request)
        {
            return _scheduleManager.GetByDeviceIds(request);
        }

        [HttpPost]
        public Task RemoveSchedule([FromBody] RemoveScheduleRequest request)
        {
            return _scheduleRepository.Remove(request.ScheduleId);
        }

        [HttpPost]
        public Task UpdateScheduleTime([FromBody] UpdateScheduleTimeRequest request)
        {
            if (request.Minute % 5 != 0)
            {
                throw new ArgumentException(request.Minute.ToString());
            }

            return _scheduleRepository.UpdateTime(request.ScheduleId, request.Hour, request.Minute);
        }
    }
}
