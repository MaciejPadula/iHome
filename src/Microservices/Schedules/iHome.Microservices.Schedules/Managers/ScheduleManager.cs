using iHome.Microservices.Schedules.Contract.Models;
using iHome.Microservices.Schedules.Contract.Models.Request;
using iHome.Microservices.Schedules.Contract.Models.Response;
using iHome.Microservices.Schedules.Helpers;
using iHome.Microservices.Schedules.Infrastructure.Repositories;
using iHome.Microservices.Schedules.Providers;

namespace iHome.Microservices.Schedules.Managers;

public class ScheduleManager : IScheduleManager
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly ISchedulesRunnedSetter _schedulesRunnedSetter;

    public ScheduleManager(IScheduleRepository scheduleRepository, ISchedulesRunnedSetter schedulesRunnedSetter)
    {
        _scheduleRepository = scheduleRepository;
        _schedulesRunnedSetter = schedulesRunnedSetter;
    }

    public async Task<GetSchedulesResponse> GetByDeviceIds(GetSchedulesWithDevicesRequest request)
    {
        var schedules = await _scheduleRepository.GetByDevicesIds(request.DeviceIds);

        return new()
        {
            Schedules = await _schedulesRunnedSetter.SetRunnedProperty(schedules)
        };
    }

    public async Task<GetSchedulesResponse> GetSchedules(GetSchedulesRequest request)
    {
        var schedules = await _scheduleRepository.GetByUserId(request.UserId);

        return new()
        {
            Schedules = await _schedulesRunnedSetter.SetRunnedProperty(schedules)
        };
    }
}
