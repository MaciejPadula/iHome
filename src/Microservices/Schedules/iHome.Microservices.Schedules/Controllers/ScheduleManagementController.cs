using iHome.Microservices.Schedules.Contract;
using iHome.Microservices.Schedules.Contract.Models.Request;
using iHome.Microservices.Schedules.Contract.Models.Response;
using iHome.Microservices.Schedules.Helpers;
using iHome.Microservices.Schedules.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.Schedules.Controllers
{
    public class ScheduleManagementController : ControllerBase, IScheduleManagementService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IScheduleRunHistoryRepository _scheduleRunHistoryRepository;

        public ScheduleManagementController(IScheduleRepository scheduleRepository, IScheduleRunHistoryRepository schedulesRunHistoryRepository)
        {
            _scheduleRepository = scheduleRepository;
            _scheduleRunHistoryRepository = schedulesRunHistoryRepository;
        }

        [HttpPost]
        public Task AddSchedule(AddScheduleRequest request)
        {
            return _scheduleRepository.Add(request.ScheduleName, request.Hour, request.Minute, request.UserId);
        }

        [HttpPost]
        public async Task<GetScheduleResponse> GetSchedule(GetScheduleRequest request)
        {
            var schedule = await _scheduleRepository.GetById(request.ScheduleId);

            schedule.Runned = await _scheduleRunHistoryRepository.ScheduleRunned(request.ScheduleId, DateTime.UtcNow.StartOfDay());

            return new()
            {
                Schedule = schedule
            };
        }

        [HttpPost]
        public async Task<GetSchedulesResponse> GetSchedules(GetSchedulesRequest request)
        {
            var utcNow = DateTime.UtcNow.StartOfDay();
            var schedules = await _scheduleRepository.GetByUserId(request.UserId);

            foreach(var schedule in schedules)
            {
                schedule.Runned = await _scheduleRunHistoryRepository.ScheduleRunned(schedule.Id, utcNow);
            }

            return new()
            {
                Schedules = schedules
            };
        }

        [HttpPost]
        public Task RemoveSchedule(RemoveScheduleRequest request)
        {
            return _scheduleRepository.Remove(request.ScheduleId);
        }

        [HttpPost]
        public Task UpdateScheduleTime(UpdateScheduleTimeRequest request)
        {
            if (request.Minute % 5 != 0)
            {
                throw new ArgumentException(request.Minute.ToString());
            }

            return _scheduleRepository.UpdateTime(request.ScheduleId, request.Hour, request.Minute);
        }
    }
}
