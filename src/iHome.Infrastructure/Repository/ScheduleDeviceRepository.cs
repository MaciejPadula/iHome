using iHome.Infrastructure.Mappers;
using iHome.Microservices.Schedules.Contract;
using iHome.Model;
using iHome.Repository;

namespace iHome.Infrastructure.Repository;

internal class ScheduleDeviceRepository : IScheduleDeviceRepository
{
    private readonly IScheduleDeviceManagementService _scheduleDeviceManagementService;
    private readonly IScheduleManagementService _scheduleManagementService;

    public ScheduleDeviceRepository(IScheduleDeviceManagementService scheduleDeviceManagementService)
    {
        _scheduleDeviceManagementService = scheduleDeviceManagementService;
    }

    public async Task Add(ScheduleDeviceDto scheduleDevice)
    {
        await _scheduleDeviceManagementService.AddOrUpdateDeviceSchedule(new()
        {
            DeviceId = scheduleDevice.DeviceId,
            ScheduleId = scheduleDevice.ScheduleId,
            DeviceData = scheduleDevice.Data
        });
    }

    public async Task<IEnumerable<ScheduleDeviceDto>> GetScheduleDevices(Guid scheduleId)
    {
        var response = await _scheduleDeviceManagementService.GetScheduleDevices(new()
        {
            ScheduleId = scheduleId
        });

        return response?
            .ScheduleDevices?
            .Select(sd => sd.ToDto()) ?? Enumerable.Empty<ScheduleDeviceDto>();
    }

    public async Task<IEnumerable<Guid>> GetUserScheduleDevices(string userId)
    {
        var response = await _scheduleDeviceManagementService.GetDevicesForScheduling(new()
        {
            UserId = userId
        });

        return response?.DeviceIds ?? Enumerable.Empty<Guid>();
    }

    public async Task Remove(Guid deviceId, Guid scheduleId)
    {
        await _scheduleDeviceManagementService.RemoveDeviceSchedule(new()
        {
            DeviceId = deviceId,
            ScheduleId = scheduleId
        });
    }

    public async Task UpdateScheduleDeviceData(ScheduleDeviceDto scheduleDevice)
    {
        await _scheduleDeviceManagementService.AddOrUpdateDeviceSchedule(new()
        {
            DeviceId = scheduleDevice.DeviceId,
            ScheduleId = scheduleDevice.ScheduleId,
            DeviceData = scheduleDevice.Data
        });
    }
}
