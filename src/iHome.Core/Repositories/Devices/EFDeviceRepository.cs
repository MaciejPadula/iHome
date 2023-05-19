using iHome.Core.Models;
using iHome.Infrastructure.SQL.Contexts;
using iHome.Infrastructure.SQL.Models.Enums;
using iHome.Infrastructure.SQL.Models.RootTables;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Repositories.Devices;

public class EFDeviceRepository : IDeviceRepository
{
    private readonly SqlDataContext _sqlDataContext;

    public EFDeviceRepository(SqlDataContext sqlDataContext)
    {
        _sqlDataContext = sqlDataContext;
    }

    public async Task<Guid> Add(string name, string macAddress, DeviceType type, Guid roomId)
    {
        var device = await _sqlDataContext.Devices.AddAsync(new Device
        {
            Name = name,
            Type = type,
            RoomId = roomId,
            MacAddress = macAddress
        });

        await _sqlDataContext.SaveChangesAsync();
        return device.Entity.Id;
    }

    public Task ChangeRoom(Guid deviceId, Guid roomId)
    {
        return UpdateDevice(deviceId, (ctx, d) => d.RoomId = roomId);
    }

    public Task<List<DeviceModel>> GetByDeviceId(Guid deviceId)
    {
        return _sqlDataContext.Devices
            .Where(d => d.Id == deviceId)
            .Select(d => new DeviceModel(d))
            .ToListAsync();
    }

    public Task<List<DeviceModel>> GetByRoomId(Guid roomId)
    {
        return _sqlDataContext.Devices
            .Where(d => d.RoomId == roomId)
            .Select(d => new DeviceModel(d))
            .ToListAsync();
    }

    public Task<List<DeviceModel>> GetByUserId(string userId)
    {
        return _sqlDataContext.Devices
            .Include(d => d.Room)
            .Where(d => d.Room != null && d.Room.UserId == userId)
            .Select(d => new DeviceModel(d))
            .ToListAsync();
    }

    public Task Remove(Guid deviceId)
    {
        return UpdateDevice(deviceId, (ctx, d) => ctx.Remove(d));
    }

    public Task Rename(Guid deviceId, string name)
    {
        return UpdateDevice(deviceId, (ctx, d) => d.Name = name);
    }

    private async Task UpdateDevice(Guid deviceId, Action<SqlDataContext, Device> updater)
    {
        var device = await _sqlDataContext.Devices
            .FirstOrDefaultAsync(d => d.Id == deviceId);

        if (device == null) return;

        updater.Invoke(_sqlDataContext, device);

        await _sqlDataContext.SaveChangesAsync();
    }
}
