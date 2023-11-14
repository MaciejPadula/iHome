using iHome.Core.Helpers;
using iHome.Core.Services.Validation;
using iHome.Microservices.Schedules.Contract;
using iHome.Microservices.Schedules.Contract.Models;
using iHome.Microservices.Schedules.Contract.Models.Request;
using iHome.Model;

namespace iHome.Core.Services;

public interface IScheduleService
{
    Task<Guid> AddOrUpdateScheduleDevice(Guid scheduleId, Guid deviceId, string deviceData, string userId);
    Task<List<ScheduleDeviceModel>> GetScheduleDevices(Guid scheduleId, string userId);
}

public class ScheduleService : IScheduleService
{
    private readonly IScheduleDeviceManagementService _scheduleDeviceManagementService;
    private readonly IValidationService _validationService;

    public ScheduleService(IScheduleDeviceManagementService scheduleDeviceManagementService,
        IValidationService validationService)
    {
        _scheduleDeviceManagementService = scheduleDeviceManagementService;
        _validationService = validationService;
    }

    public async Task<Guid> AddOrUpdateScheduleDevice(Guid scheduleId, Guid deviceId, string deviceData, string userId)
    {
        var request = new AddOrUpdateDeviceScheduleRequest
        {
            DeviceId = deviceId,
            ScheduleId = scheduleId,
            DeviceData = deviceData
        };

        var response = await _validationService.Validate(scheduleId, userId, ValidatorType.Schedule, () => _scheduleDeviceManagementService.AddOrUpdateDeviceSchedule(request));
        return response.ScheduleDeviceId;
    }

    public async Task<List<ScheduleDeviceModel>> GetScheduleDevices(Guid scheduleId, string userId)
    {
        var request = new GetScheduleDevicesRequest
        {
            ScheduleId = scheduleId
        };

        var scheduleDevices = await _validationService.Validate(scheduleId, userId, ValidatorType.Schedule, () => _scheduleDeviceManagementService.GetScheduleDevices(request));

        return scheduleDevices?
            .ScheduleDevices?
            .ToList() ?? ListUtils.Empty<ScheduleDeviceModel>();
    }
}
