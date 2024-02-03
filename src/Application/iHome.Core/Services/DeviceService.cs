using iHome.Core.Helpers;
using iHome.Model;
using iHome.Core.Services.Validation;
using iHome.Microservices.Devices.Contract;
using iHome.Microservices.Devices.Contract.Models;
using iHome.Microservices.Devices.Contract.Models.Request;
using iHome.Microservices.Schedules.Contract;
using iHome.Microservices.Schedules.Contract.Models;
using iHome.Microservices.Schedules.Contract.Models.Request;

namespace iHome.Core.Services;

public interface IDeviceService
{
    Task AddDevice(string name, string macAddress, DeviceType type, string userId, Guid roomId);
    Task ChangeDeviceName(Guid deviceId, string name, string userId);
    Task ChangeDeviceRoom(Guid deviceId, Guid roomId, string userId);
    Task<List<DeviceModel>> GetDevices(Guid? roomId, string userId);
    Task<List<ScheduleModel>> GetDeviceSchedules(Guid deviceId, string userId);
    Task RemoveDevice(Guid deviceId, string userId);

    Task<string> GetDeviceData(Guid deviceId, string userId);
    Task SetDeviceData(Guid deviceId, string deviceData, string userId);
}

public class DeviceService : IDeviceService
{
    private readonly IDeviceManagementService _deviceManagementService;
    private readonly IDeviceDataService _deviceDataService;
    private readonly IScheduleManagementService _scheduleManagementService;

    private readonly IValidationService _validationService;

    public DeviceService(IDeviceManagementService deviceManagementService,
        IDeviceDataService deviceDataService,
        IValidationService validationService,
        IScheduleManagementService scheduleManagementService)
    {
        _deviceManagementService = deviceManagementService;
        _deviceDataService = deviceDataService;
        _validationService = validationService;
        _scheduleManagementService = scheduleManagementService;
    }

    public async Task AddDevice(string name, string macAddress, DeviceType type, string userId, Guid roomId)
    {
        var request = new AddDeviceRequest
        {
            Name = name,
            MacAddress = name,
            Type = type,
            RoomId = roomId
        };

        await _validationService.Validate(roomId, userId, ValidatorType.RoomWrite, () => _deviceManagementService.AddDevice(request));
    }

    public async Task ChangeDeviceName(Guid deviceId, string name, string userId)
    {
        var request = new RenameDeviceRequest
        {
            DeviceId = deviceId,
            NewName = name
        };

        await _validationService.Validate(deviceId, userId, ValidatorType.DeviceWrite, () => _deviceManagementService.RenameDevice(request));
    }

    public async Task ChangeDeviceRoom(Guid deviceId, Guid roomId, string userId)
    {
        var request = new ChangeDeviceRoomRequest
        {
            RoomId = roomId,
            DeviceId = deviceId
        };

        await _validationService.Validate(deviceId, userId, ValidatorType.DeviceWrite, () => _deviceManagementService.ChangeDeviceRoom(request));
    }

    public async Task<string> GetDeviceData(Guid deviceId, string userId)
    {
        var request = new GetDeviceDataRequest
        {
            DeviceId = deviceId
        };

        var data = await _validationService.Validate(deviceId, userId, ValidatorType.DeviceRead, () => _deviceDataService.GetDeviceData(request));

        return data?.DeviceData ?? "{}"; 
    }

    public async Task<List<DeviceModel>> GetDevices(Guid? roomId, string userId)
    {
        if (roomId.HasValue)
        {
            await _validationService.Validate(roomId.Value, userId, ValidatorType.RoomRead, () => Task.CompletedTask);
        }

        var request = new GetDevicesRequest
        {
            RoomId = roomId ?? default,
            UserId = userId
        };

        var devices = await _deviceManagementService.GetDevices(request);

        return devices?.Devices?.ToList() ?? Enumerable.Empty<DeviceModel>().ToList();
    }

    public async Task<List<ScheduleModel>> GetDeviceSchedules(Guid deviceId, string userId)
    {
        var request = new GetSchedulesWithDevicesRequest
        {
            DeviceIds = new List<Guid> { deviceId }
        };

        var response = await _validationService.Validate(deviceId, userId, ValidatorType.DeviceRead, () =>
            _scheduleManagementService.GetSchedulesWithDevices(request));

        return response?.Schedules?
            .OrderBy(s => s.Hour)?
            .ThenBy(s => s.Minute)?
            .ToList() ?? ListUtils.Empty<ScheduleModel>();
    }

    public async Task RemoveDevice(Guid deviceId, string userId)
    {
        var request = new RemoveDeviceRequest
        {
            DeviceId = deviceId
        };

        await _validationService.Validate(deviceId, userId, ValidatorType.DeviceWrite, () => _deviceManagementService.RemoveDevice(request));
    }

    public async Task SetDeviceData(Guid deviceId, string deviceData, string userId)
    {
        var request = new SetDeviceDataRequest
        {
            DeviceId = deviceId,
            Data = deviceData
        };

        await _validationService.Validate(deviceId, userId, ValidatorType.DeviceWrite, () => _deviceDataService.SetDeviceData(request));
    }
}
