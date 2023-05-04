using iHome.Core.Exceptions.SqlExceptions;
using iHome.Core.Models;
using iHome.Core.Services.Devices;
using iHome.Infrastructure.SQL.Contexts;
using iHome.Infrastructure.SQL.Models.ConnectionTables;
using iHome.Infrastructure.SQL.Models.RootTables;
using iHome.Shared.Logic;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Services.Schedules;

public class ScheduleService : IScheduleService
{
    private readonly IDeviceService _deviceService;
    private readonly SqlDataContext _context;

    public ScheduleService(IDeviceService deviceService, SqlDataContext context)
    {
        _deviceService = deviceService;
        _context = context;
    }

    public async Task AddOrUpdateDeviceSchedule(Guid scheduleId, Guid deviceId, string deviceData, string userId)
    {
        if (!await _deviceService.DeviceExists(deviceId, userId))
        {
            throw new DeviceNotFoundException();
        }

        ScheduleDevice? scheduleDevice;

        try
        {
            scheduleDevice = await GetScheduleDeviceById(scheduleId, deviceId, userId);
        }
        catch (DeviceNotFoundException)
        {
            scheduleDevice = (await _context.ScheduleDevices.AddAsync(
                new ScheduleDevice
                {
                    ScheduleId = scheduleId,
                    DeviceId = deviceId,
                    DeviceData = deviceData
                })).Entity;
        }

        scheduleDevice.DeviceData = deviceData;

        await _context.SaveChangesAsync();

    }

    public async Task<ScheduleModel> GetSchedule(Guid scheduleId, string userId)
    {
        var schedule = await GetScheduleById(scheduleId, userId);

        return new ScheduleModel(schedule);
    }

    public async Task<ScheduleModel> GetScheduleWithDevices(Guid scheduleId, string userId)
    {
        var schedule = await GetScheduleByIdWithDevices(scheduleId, userId);

        return new ScheduleModel(schedule);
    }

    public async Task<int> GetDevicesInScheduleCount(Guid scheduleId, string userId)
    {
        var schedule = await GetScheduleByIdWithDevices(scheduleId, userId);

        return schedule.ScheduleDevices?.Count ?? 0;
    }

    public async Task<ScheduleDeviceModel> GetScheduleDevice(Guid scheduleId, Guid deviceId, string userId)
    {
        var scheduleDevice = await _context.ScheduleDevices
            .Include(s => s.Device)
            .SingleOrDefaultAsync(s => s.ScheduleId == scheduleId && s.DeviceId == deviceId);

        if (scheduleDevice == null) throw new DeviceNotFoundException();

        return new ScheduleDeviceModel(scheduleDevice);
    }

    public async Task<IEnumerable<ScheduleDeviceModel>> GetScheduleDevices(Guid scheduleId, string userId)
    {
        var schedule = await GetScheduleByIdWithDevices(scheduleId, userId);

        return schedule.ScheduleDevices
            .Select(s => new ScheduleDeviceModel(s));
    }

    public Task<IEnumerable<ScheduleModel>> GetSchedules(string userId)
    {
        var schedules = _context.Schedules
            .Where(s => s.UserId == userId)
            .Include(s => s.ScheduleRuns)
            .Select(s => new ScheduleModel(s))
            .AsEnumerable();

        return Task.FromResult(schedules);
    }

    public async Task RemoveDeviceSchedule(Guid scheduleId, Guid deviceId, string userId)
    {
        var scheduleToRemove = await GetScheduleDeviceById(scheduleId, deviceId, userId);

        _context.ScheduleDevices.Remove(scheduleToRemove);
        await _context.SaveChangesAsync();
    }

    public async Task AddSchedule(string scheduleName, int day, int hour, int minute, string userId)
    {
        var schedule = new Schedule
        {
            Name = scheduleName,
            UserId = userId,
            ActivationCron = CronHelper.CreateCronExpressions(day, hour, minute)
        };

        await _context.Schedules.AddAsync(schedule);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveSchedule(Guid scheduleId, string userId)
    {
        var scheduleToRemove = await GetScheduleByIdWithDevices(scheduleId, userId);

        _context.ScheduleDevices.RemoveRange(scheduleToRemove.ScheduleDevices);
        _context.Schedules.Remove(scheduleToRemove);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateScheduleTime(Guid scheduleId, int day, int hour, int minute, string userId)
    {
        var schedule = await GetScheduleById(scheduleId, userId);

        schedule.ActivationCron = CronHelper.CreateCronExpressions(day, hour, minute);

        await _context.SaveChangesAsync();
    }

    private Task<IQueryable<Schedule>> GetSchedulesQueryAsync(Guid scheduleId, string userId)
    {
        return Task.FromResult(_context.Schedules
            .Where(s => s.Id == scheduleId && s.UserId == userId));
    }

    private async Task<Schedule> GetScheduleById(Guid scheduleId, string userId)
    {
        var schedule = (await GetSchedulesQueryAsync(scheduleId, userId))
            .FirstOrDefault();

        return schedule ?? throw new ScheduleNotFoundException();
    }

    private async Task<Schedule> GetScheduleByIdWithDevices(Guid scheduleId, string userId)
    {
        var schedule = (await GetSchedulesQueryAsync(scheduleId, userId))
            .Include(s => s.ScheduleDevices)
            .ThenInclude(d => d.Device)
            .FirstOrDefault();

        return schedule ?? throw new ScheduleNotFoundException();
    }

    private async Task<ScheduleDevice> GetScheduleDeviceById(Guid scheduleId, Guid deviceId, string userId)
    {
        var device = await _context.ScheduleDevices
            .Include(d => d.Schedule)
            .Where(d => d.ScheduleId == scheduleId && d.DeviceId == deviceId && d.Schedule != null && d.Schedule.UserId == userId)
            .FirstOrDefaultAsync();

        return device ?? throw new DeviceNotFoundException();
    }
}
