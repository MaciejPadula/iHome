using iHome.Microservices.Schedules.Contract.Models.Request;
using iHome.Microservices.Schedules.Contract.Models.Response;
using iHome.Microservices.Schedules.Infrastructure.Repositories;

namespace iHome.Microservices.Schedules.Managers;

public class ScheduleManager : IScheduleManager
{
    private readonly IScheduleRepository _scheduleRepository;

    public ScheduleManager(IScheduleRepository scheduleRepository)
    {
        _scheduleRepository = scheduleRepository;
    }

    public async Task<GetSchedulesResponse> GetByDeviceIds(GetSchedulesWithDevicesRequest request)
    {
        return new()
        {
            Schedules = await _scheduleRepository.GetByDevicesIds(request.DeviceIds)
        };
    }
}
