using iHome.Microservices.Devices.Contract;
using iHome.Microservices.Schedules.Contract;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.DevicesScheduling.Features.GetDevicesForScheduling;

internal class GetDevicesForSchedulingQueryHandler : IAsyncQueryHandler<GetDevicesForSchedulingQuery>
{
    private readonly IScheduleDeviceManagementService _scheduleDeviceManagementService;
    private readonly IDeviceManagementService _deviceManagementService;
    private readonly IDeviceDataService _deviceDataService;

    public GetDevicesForSchedulingQueryHandler(IScheduleDeviceManagementService scheduleDeviceManagementService, IDeviceManagementService deviceManagementService, IDeviceDataService deviceDataService)
    {
        _scheduleDeviceManagementService = scheduleDeviceManagementService;
        _deviceManagementService = deviceManagementService;
        _deviceDataService = deviceDataService;
    }

    public async Task HandleAsync(GetDevicesForSchedulingQuery query)
    {
        var deviceIdsForScheduling = await _scheduleDeviceManagementService.GetDevicesForScheduling(new()
        {
            UserId = query.UserId
        });

        if (deviceIdsForScheduling is null)
        {
            query.Result = [];
            return;
        }

        var devices = await _deviceManagementService.GetDevicesByIds(new()
        {
            UserId = query.UserId,
            DeviceIds = deviceIdsForScheduling.DeviceIds
        });

        if (devices?.Devices is null)
        {
            query.Result = [];
            return;
        }

        foreach (var device in devices.Devices)
        {
            device.Data = (await _deviceDataService.GetDeviceData(new()
            {
                DeviceId = device.Id
            })).DeviceData;
        }

        query.Result = devices.Devices?
            .Select(x => new Model.DeviceDto
            {
                Id = x.Id,
                Name = x.Name,
                Type = (int)x.Type,
            })?
            .ToList() ?? [];
    }
}
