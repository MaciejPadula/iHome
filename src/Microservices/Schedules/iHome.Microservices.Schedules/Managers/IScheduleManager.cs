using iHome.Microservices.Schedules.Contract.Models.Request;
using iHome.Microservices.Schedules.Contract.Models.Response;

namespace iHome.Microservices.Schedules.Managers;

public interface IScheduleManager
{
    Task<GetSchedulesResponse> GetByDeviceIds(GetSchedulesWithDevicesRequest request);
}
