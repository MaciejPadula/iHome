using iHome.Core.Services.Validation;
using iHome.Microservices.Schedules.Contract;
using iHome.Microservices.Schedules.Contract.Models.Request;
using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.DevicesScheduling.Features.ScheduleOrUpdateDevice;

internal class ScheduleOrUpdateDeviceQueryHandler : IAsyncQueryHandler<ScheduleOrUpdateDeviceQuery>
{
    private readonly IScheduleDeviceManagementService _scheduleDeviceManagementService;
    private readonly IValidationService _validationService;

    public ScheduleOrUpdateDeviceQueryHandler(IScheduleDeviceManagementService scheduleDeviceManagementService, IValidationService validationService)
    {
        _scheduleDeviceManagementService = scheduleDeviceManagementService;
        _validationService = validationService;
    }

    public async Task HandleAsync(ScheduleOrUpdateDeviceQuery command)
    {
        var request = new AddOrUpdateDeviceScheduleRequest
        {
            DeviceId = command.DeviceId,
            ScheduleId = command.ScheduleId,
            DeviceData = command.DeviceData
        };

        var response = await _validationService.Validate(
            command.DeviceId,
            command.UserId,
            ValidatorType.Schedule,
            () => _scheduleDeviceManagementService.AddOrUpdateDeviceSchedule(request));

        command.Result = response.ScheduleDeviceId;
    }
}
