﻿using iHome.Core.Exceptions.SqlExceptions;
using iHome.Core.Models;
using iHome.Core.Services.Rooms;
using iHome.Infrastructure.Firebase.Repositories;
using iHome.Infrastructure.SQL.Contexts;
using iHome.Infrastructure.SQL.Models.Enums;
using iHome.Infrastructure.SQL.Models.RootTables;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Services.Devices;

public class DeviceService : IDeviceService
{
    private readonly IRoomService _roomService;
    private readonly IDeviceDataRepository _deviceDataRepository;
    private readonly SqlDataContext _sqlDataContext;

    public DeviceService(IRoomService roomService, IDeviceDataRepository deviceDataRepository, SqlDataContext sqlDataContext)
    {
        _roomService = roomService;
        _deviceDataRepository = deviceDataRepository;
        _sqlDataContext = sqlDataContext;
    }

    public async Task<Guid> AddDevice(string name, string macAddress, DeviceType type, Guid roomId, string userId)
    {
        if (!await _roomService.UserCanAccessRoom(roomId, userId))
        {
            throw new RoomNotFoundException();
        }

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

    public async Task ChangeDeviceRoom(Guid deviceId, Guid roomId, string userId)
    {
        if (!await _roomService.UserCanAccessRoom(roomId, userId))
        {
            throw new RoomNotFoundException();
        }

        var device = await GetDevice(deviceId, userId);
        device.RoomId = roomId;

        await _sqlDataContext.SaveChangesAsync();
    }

    public async Task<DeviceModel> GetDevice(Guid deviceId, string userId)
    {
        var device = await _sqlDataContext.Devices.FirstOrDefaultAsync(d => d.Id == deviceId);
        if (device == null || !await CanGetDevice(device, userId))
        {
            throw new DeviceNotFoundException();
        }

        return new DeviceModel(device);
    }

    public Task<bool> DeviceExists(Guid deviceId, string userId)
    {
        return _sqlDataContext.Devices
            .Include(d => d.Room)
            .AnyAsync(d => d.Id == deviceId && d.Room != null && d.Room.UserId == userId);
    }

    public async Task<IEnumerable<DeviceModel>> GetDevices(Guid roomId, string userId)
    {

        var selectedRoom = await _sqlDataContext.Rooms
            .Where(room => room.Id == roomId && room.UserId == userId)
            .Include(room => room.Devices)
            .FirstOrDefaultAsync() ?? throw new RoomNotFoundException();

        return selectedRoom.Devices
            .Select(d => new DeviceModel(d)) ?? Enumerable.Empty<DeviceModel>();
    }

    public async Task<IEnumerable<DeviceModel>> GetDevices(string userId)
    {
        var devices = await _sqlDataContext.Devices
            .Include(d => d.Room)
            .Where(d => d.Room != null && d.Room.UserId == userId)
            .Select(d => new DeviceModel(d))
            .ToListAsync();

        return devices;
    }

    public async Task RemoveDevice(Guid deviceId, string userId)
    {
        var device = await _sqlDataContext.Devices.FirstOrDefaultAsync(d => d.Id == deviceId) ?? throw new DeviceNotFoundException();

        if (!await CanGetDevice(device, userId))
        {
            throw new DeviceNotFoundException();
        }

        _sqlDataContext.Devices.Remove(device);
        await _sqlDataContext.SaveChangesAsync();
    }

    public async Task RenameDevice(Guid deviceId, string newName, string userId)
    {
        var device = await GetDevice(deviceId, userId);
        device.Name = newName;

        await _sqlDataContext.SaveChangesAsync();
    }

    public async Task SetDeviceData(Guid deviceId, string data, string userId)
    {
        var device = await GetDevice(deviceId, userId);

        await _deviceDataRepository.SetData(device.MacAddress, data);
    }

    public async Task<string> GetDeviceData(Guid deviceId, string userId)
    {
        var device = await GetDevice(deviceId, userId);

        return await _deviceDataRepository.GetData(device.MacAddress);
    }

    private async Task<bool> CanGetDevice(Device device, string userId)
    {
        return await _roomService.UserCanAccessRoom(device.RoomId, userId);
    }
}
