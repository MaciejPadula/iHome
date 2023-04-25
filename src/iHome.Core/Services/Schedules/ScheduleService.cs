using iHome.Core.Exceptions.SqlExceptions;
using iHome.Core.Services.Devices;
using iHome.Infrastructure.SQL.Contexts;
using iHome.Infrastructure.SQL.Models;
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
            scheduleDevice = await GetScheduleDevice(scheduleId, deviceId, userId);
        }
        catch (DeviceNotFoundException ex)
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

    public async Task<Schedule> GetSchedule(Guid scheduleId, string userId)
    {
        var schedule = await _context.Schedules.FirstOrDefaultAsync(s => s.Id == scheduleId && s.UserId == userId);

        if (schedule == null)
        {
            throw new ScheduleNotFoundException();
        }

        return schedule;
    }

    public async Task<Schedule> GetScheduleWithDevices(Guid scheduleId, string userId)
    {
        var schedule = await _context.Schedules
            .Include(s => s.ScheduleDevices)
            .ThenInclude(d => d.Device)
            .FirstOrDefaultAsync(s => s.Id == scheduleId && s.UserId == userId);

        if (schedule == null)
        {
            throw new ScheduleNotFoundException();
        }

        return schedule;
    }

    public async Task<int> GetDevicesInScheduleCount(Guid scheduleId, string userId)
    {
        var schedule = (await GetSchedulesQueryAsync(scheduleId, userId))
            .Include(s => s.ScheduleDevices)
            .FirstOrDefault();

        if (schedule == null)
        {
            throw new ScheduleNotFoundException();
        }

        return schedule.ScheduleDevices?.Count ?? 0;
    }

    private Task<IQueryable<Schedule>> GetSchedulesQueryAsync(Guid scheduleId, string userId)
    {
        return Task.FromResult(_context.Schedules
            .Where(s => s.Id == scheduleId && s.UserId == userId));
    }

    public async Task<ScheduleDevice> GetScheduleDevice(Guid scheduleId, Guid deviceId, string userId)
    {
        var scheduleDevice = await _context.ScheduleDevices
            .Include(s => s.Device)
            .SingleOrDefaultAsync(s => s.ScheduleId == scheduleId && s.DeviceId == deviceId);

        if (scheduleDevice == null ||
            scheduleDevice.Device == null ||
            !await _context.Schedules.AnyAsync(s => s.Id == scheduleId && s.UserId == userId))
        {
            throw new DeviceNotFoundException();
        }

        return scheduleDevice;
    }

    public async Task<IEnumerable<ScheduleDevice>> GetScheduleDevices(Guid scheduleId, string userId)
    {
        var schedule = (await GetSchedulesQueryAsync(scheduleId, userId))
            .Include(s => s.ScheduleDevices)
            .ThenInclude(s => s.Device)
            .SingleOrDefault();

        return schedule == null ? throw new Exception() : schedule.ScheduleDevices
            .Where(s => s.Device != null);
    }

    public Task<IEnumerable<Schedule>> GetSchedules(string userId)
    {
        var schedules = _context.Schedules
            .Where(s => s.UserId == userId)
            .AsEnumerable();

        return Task.FromResult(schedules);
    }

    public Task<IEnumerable<Guid>> GetScheduleIds(string userId)
    {
        var schedules = _context.Schedules
            .Where(s => s.UserId == userId)
            .Select(s => s.Id)
            .AsEnumerable();

        return Task.FromResult(schedules);
    }

    public async Task RemoveDeviceSchedule(Guid scheduleId, Guid deviceId, string userId)
    {
        var scheduleToRemove = await GetScheduleDevice(scheduleId, deviceId, userId) ?? throw new Exception();
        _context.Remove(scheduleToRemove);
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
        var scheduleToRemove = await GetSchedule(scheduleId, userId);
        var scheduleDevices = await GetScheduleDevices(scheduleId, userId);

        _context.ScheduleDevices.RemoveRange(scheduleDevices);
        _context.Schedules.Remove(scheduleToRemove);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateScheduleTime(Guid scheduleId, int day, int hour, int minute, string userId)
    {
        var schedule = await GetSchedule(scheduleId, userId);
        schedule.ActivationCron = CronHelper.CreateCronExpressions(day, hour, minute);

        await _context.SaveChangesAsync();
    }
}
