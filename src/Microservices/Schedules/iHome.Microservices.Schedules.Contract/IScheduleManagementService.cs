using iHome.Microservices.Schedules.Contract.Models;
using iHome.Microservices.Schedules.Contract.Models.Request;
using iHome.Microservices.Schedules.Contract.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iHome.Microservices.Schedules.Contract
{
    public interface IScheduleManagementService
    {
        Task AddSchedule(AddScheduleRequest request);

        Task<GetSchedulesResponse> GetSchedules(GetSchedulesRequest request);
        Task<GetScheduleResponse> GetSchedule(GetScheduleRequest request);

        Task UpdateScheduleTime(UpdateScheduleTimeRequest request);

        Task RemoveSchedule(RemoveScheduleRequest request);
    }
}
