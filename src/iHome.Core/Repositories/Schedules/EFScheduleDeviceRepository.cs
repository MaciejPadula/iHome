using iHome.Core.Exceptions.SqlExceptions;
using iHome.Core.Models;
using iHome.Infrastructure.SQL.Contexts;
using iHome.Infrastructure.SQL.Models.ConnectionTables;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Repositories.Schedules;

public class EFScheduleDeviceRepository : IScheduleDeviceRepository
{
    private readonly SqlDataContext _sqlDataContext;

    public EFScheduleDeviceRepository(SqlDataContext sqlDataContext)
    {
        _sqlDataContext = sqlDataContext;
    }

    public Task<int> CountByScheduleId(Guid scheduleId)
    {
        return _sqlDataContext.ScheduleDevices.CountAsync(s => s.ScheduleId == scheduleId);
    }

    public async Task<ScheduleDeviceModel> GetByIdAndScheduleId(Guid deviceId, Guid scheduleId)
    {
        return await _sqlDataContext.ScheduleDevices
            .Where(d => d.DeviceId == deviceId && d.ScheduleId == scheduleId)
            .Include(d => d.Device)
            .Select(d => new ScheduleDeviceModel(d))
            .FirstOrDefaultAsync() ?? throw new DeviceNotFoundException();
    }

    public Task<List<ScheduleDeviceModel>> GetByScheduleId(Guid scheduleId)
    {
        return _sqlDataContext.ScheduleDevices
            .Where(d => d.ScheduleId == scheduleId)
            .Include(d => d.Device)
            .Select(d => new ScheduleDeviceModel(d))
            .ToListAsync();
    }

    public async Task Remove(Guid scheduleId, Guid deviceId)
    {
        var scheduleDevice = _sqlDataContext.ScheduleDevices
            .Where(d => d.DeviceId == deviceId && d.ScheduleId == scheduleId)
            .FirstOrDefaultAsync() ?? throw new DeviceNotFoundException();

        _sqlDataContext.Remove(scheduleDevice);
        await _sqlDataContext.SaveChangesAsync();
    }

    public async Task Upsert(Guid scheduleId, Guid deviceId, string deviceData)
    {
        var scheduleDevice = await _sqlDataContext.ScheduleDevices
            .FirstOrDefaultAsync(d => d.ScheduleId == scheduleId && d.DeviceId == deviceId);

        scheduleDevice ??= new ScheduleDevice
        {
            ScheduleId = scheduleId,
            DeviceId = deviceId,
            DeviceData = deviceData
        };

        scheduleDevice.DeviceData = deviceData;

        _sqlDataContext.Update(scheduleDevice);

        await _sqlDataContext.SaveChangesAsync();
    }
}
