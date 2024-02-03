using iHome.Core.Helpers;
using iHome.Microservices.Schedules.Contract.Models.Request;
using iHome.Microservices.Schedules.Contract.Models;
using iHome.Microservices.UsersApi.Contract.Models;
using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Query;
using iHome.Microservices.Schedules.Contract;
using iHome.Core.Services.Validation;

namespace iHome.DevicesScheduling.Features.GetScheduleDevices;

internal class GetScheduleDevicesQueryHandler : IAsyncQueryHandler<GetScheduleDevicesQuery>
{
    private readonly IScheduleDeviceManagementService _scheduleDeviceManagementService;
    private readonly IValidationService _validationService;

    public GetScheduleDevicesQueryHandler(IScheduleDeviceManagementService scheduleDeviceManagementService, IValidationService validationService)
    {
        _scheduleDeviceManagementService = scheduleDeviceManagementService;
        _validationService = validationService;
    }

    public async Task HandleAsync(GetScheduleDevicesQuery query)
    {
        var request = new GetScheduleDevicesRequest
        {
            ScheduleId = query.ScheduleId
        };

        var scheduleDevices = await _validationService.Validate(
            query.ScheduleId,
            query.UserId,
            ValidatorType.Schedule,
            () => _scheduleDeviceManagementService.GetScheduleDevices(request));

        query.Result = scheduleDevices?
            .ScheduleDevices?
            .Select(x => new ScheduleDeviceDto
            {
                Id = x.Id,
                DeviceId = x.DeviceId,
                Data = x.DeviceData,
                ScheduleId = x.ScheduleId,
                Name = x.Name,
                Type = x.Type
            })?
            .ToList() ?? [];
    }
}
