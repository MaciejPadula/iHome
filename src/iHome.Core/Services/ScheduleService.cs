using iHome.Core.Helpers;
using iHome.Model;
using iHome.Core.Services.Validation;
using iHome.Microservices.Schedules.Contract;
using iHome.Microservices.Schedules.Contract.Models;
using iHome.Microservices.Schedules.Contract.Models.Request;
using iHome.Shared.Logic;

namespace iHome.Core.Services;

public interface IScheduleService
{
    Task AddSchedule(string scheduleName, int scheduleDay, string scheduleTime, string userId);
    Task<List<ScheduleModel>> GetSchedulesOrdered(string userId);
    Task<ScheduleModel> GetSchedule(Guid scheduleId, string userId);
    Task UpdateScheduleTime(Guid scheduleId, int scheduleDay, string scheduleTime, string userId);
    Task RemoveSchedule(Guid scheduleId, string userId);

    Task<Guid> AddOrUpdateScheduleDevice(Guid scheduleId, Guid deviceId, string deviceData, string userId);
    Task<List<ScheduleDeviceModel>> GetScheduleDevices(Guid scheduleId, string userId);

}

public class ScheduleService : IScheduleService
{
    private readonly IScheduleDeviceManagementService _scheduleDeviceManagementService;
    private readonly IScheduleManagementService _scheduleManagementService;
    private readonly IValidationService _validationService;
    private readonly ITimeModelParser _timeParser;

    public ScheduleService(IScheduleDeviceManagementService scheduleDeviceManagementService,
        IScheduleManagementService scheduleManagementService,
        ITimeModelParser timeParser,
        IValidationService validationService)
    {
        _scheduleDeviceManagementService = scheduleDeviceManagementService;
        _scheduleManagementService = scheduleManagementService;
        _timeParser = timeParser;
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

    public async Task AddSchedule(string scheduleName, int scheduleDay, string scheduleTime, string userId)
    {
        var runTime = _timeParser.Parse(scheduleTime);

        await _scheduleManagementService.AddSchedule(new()
        {
            ScheduleName = scheduleName,
            Day = scheduleDay,
            Hour = runTime.Hour,
            Minute = runTime.Minute,
            UserId = userId
        });
    }

    public async Task<ScheduleModel> GetSchedule(Guid scheduleId, string userId)
    {
        var request = new GetScheduleRequest
        {
            ScheduleId = scheduleId
        };

        var schedule = await _validationService.Validate(scheduleId, userId, ValidatorType.Schedule, () => _scheduleManagementService.GetSchedule(request));

        return schedule?
            .Schedule ?? throw new Exception();
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

    public async Task<List<ScheduleModel>> GetSchedulesOrdered(string userId)
    {
        var schedules = await _scheduleManagementService.GetSchedules(new()
        {
            UserId = userId
        });

        return schedules?
            .Schedules?
            .OrderBy(s => s.Hour)?
            .ThenBy(s => s.Minute)?
            .ToList() ?? ListUtils.Empty<ScheduleModel>();
    }

    public async Task RemoveSchedule(Guid scheduleId, string userId)
    {
        var request = new RemoveScheduleRequest
        {
            ScheduleId = scheduleId
        };

        await _validationService.Validate(scheduleId, userId, ValidatorType.Schedule, () => _scheduleManagementService.RemoveSchedule(request));
    }

    public async Task UpdateScheduleTime(Guid scheduleId, int scheduleDay, string scheduleTime, string userId)
    {
        var task = Task.Run(async () =>
        {
            var runTime = _timeParser.Parse(scheduleTime);

            await _scheduleManagementService.UpdateScheduleTime(new()
            {
                ScheduleId = scheduleId,
                Day = scheduleDay,
                Hour = runTime.Hour,
                Minute = runTime.Minute
            });
        });

        await _validationService.Validate(scheduleId, userId, ValidatorType.Schedule, () => task);
    }
}
