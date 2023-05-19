using iHome.Core.Models;
using iHome.Core.Repositories.Schedules;

namespace iHome.Core.Services;

public interface IScheduleService
{
    Task<IEnumerable<ScheduleModel>> GetSchedules(string userId);
    Task<ScheduleModel> GetSchedule(Guid scheduleId, string userId);
    Task<ScheduleModel> GetScheduleWithDevices(Guid scheduleId, string userId);
    Task<int> GetDevicesInScheduleCount(Guid scheduleId, string userId);

    Task AddSchedule(string scheduleName, int day, int hour, int minute, string userId);
    Task UpdateScheduleTime(Guid scheduleId, int day, int hour, int minute, string userId);
    Task RemoveSchedule(Guid scheduleId, string userId);

    Task<ScheduleDeviceModel> GetScheduleDevice(Guid scheduleId, Guid deviceId, string userId);
    Task<IEnumerable<ScheduleDeviceModel>> GetScheduleDevices(Guid scheduleId, string userId);
    Task AddOrUpdateDeviceSchedule(Guid scheduleId, Guid deviceId, string deviceData, string userId);
    Task RemoveDeviceSchedule(Guid scheduleId, Guid deviceId, string userId);
}

public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IScheduleDeviceRepository _scheduleDeviceRepository;

    public ScheduleService(IScheduleRepository scheduleRepository, IScheduleDeviceRepository scheduleDeviceRepository)
    {
        _scheduleRepository = scheduleRepository;
        _scheduleDeviceRepository = scheduleDeviceRepository;
    }

    public async Task AddOrUpdateDeviceSchedule(Guid scheduleId, Guid deviceId, string deviceData, string userId)
    {
        //if (!await _deviceAccessGuard.UserHasWriteAccess(deviceId, userId))
        //{
        //    throw new DeviceNotFoundException();
        //}

        await _scheduleDeviceRepository.Upsert(scheduleId, deviceId, deviceData);
    }

    public async Task<ScheduleModel> GetSchedule(Guid scheduleId, string userId)
    {
        //Validation


        return await _scheduleRepository.GetById(scheduleId);
    }

    public async Task<ScheduleModel> GetScheduleWithDevices(Guid scheduleId, string userId)
    {
        //Validation

        return await _scheduleRepository.GetByIdWithDevices(scheduleId);
    }

    public async Task<int> GetDevicesInScheduleCount(Guid scheduleId, string userId)
    {
        //VALIDATION

        return await _scheduleDeviceRepository.CountByScheduleId(scheduleId);
    }

    public async Task<ScheduleDeviceModel> GetScheduleDevice(Guid scheduleId, Guid deviceId, string userId)
    {
        //VALIDATION

        return await _scheduleDeviceRepository.GetByIdAndScheduleId(deviceId, scheduleId);
    }

    public async Task<IEnumerable<ScheduleDeviceModel>> GetScheduleDevices(Guid scheduleId, string userId)
    {
        //VALIDATION

        return await _scheduleDeviceRepository.GetByScheduleId(scheduleId);
    }

    public async Task<IEnumerable<ScheduleModel>> GetSchedules(string userId)
    {
        return await _scheduleRepository.GetByUserId(userId);
    }

    public async Task RemoveDeviceSchedule(Guid scheduleId, Guid deviceId, string userId)
    {
        //VALIDATION

        await _scheduleDeviceRepository.Remove(scheduleId, deviceId);
    }

    public async Task AddSchedule(string scheduleName, int day, int hour, int minute, string userId)
    {
        //Validation

        await _scheduleRepository.Add(scheduleName, hour, minute, userId);
    }

    public async Task RemoveSchedule(Guid scheduleId, string userId)
    {
        //Validation
        await _scheduleRepository.Remove(scheduleId);
    }

    public async Task UpdateScheduleTime(Guid scheduleId, int day, int hour, int minute, string userId)
    {
        //Validation

        await _scheduleRepository.UpdateTime(scheduleId, hour, minute);
    }
}
