﻿using iHome.Core.Exceptions;
using iHome.Core.Repositories;
using iHome.Core.Services.Rooms;
using iHome.Devices.Contract.Models;

namespace iHome.Core.Services.Devices;

public class DeviceService : IDeviceService
{
    private readonly IRoomService _roomService;
    private readonly SqlDataContext _sqlDataContext;

    public DeviceService(IRoomService roomService, SqlDataContext sqlDataContext)
    {
        _roomService = roomService;
        _sqlDataContext = sqlDataContext;
    }

    public Guid AddDevice(string name, string macAddress, DeviceType type, Guid hubId, Guid roomId, Guid userId)
    {
        if (!_roomService.UserCanAccessRoom(roomId, userId))
        {
            throw new RoomNotFoundException();
        }

        var deviceId = _sqlDataContext.Devices.Add(new Device
        {
            Name = name,
            Type = type,
            Data = "",
            HubId = hubId,
            RoomId = roomId,
            MacAddress = macAddress
        }).Entity.Id;

        _sqlDataContext.SaveChanges();
        return deviceId;
    }

    public void ChangeDeviceRoom(Guid deviceId, Guid roomId, Guid userId)
    {
        if(!_roomService.UserCanAccessRoom(roomId, userId))
        {
            throw new RoomNotFoundException();
        }

        var device = GetDevice(deviceId, userId);
        device.RoomId = roomId;

        _sqlDataContext.SaveChanges();
    }

    public Device GetDevice(Guid deviceId, Guid userId)
    {
        var device = _sqlDataContext.Devices.FirstOrDefault(d => d.Id == deviceId);
        if (device == null || !CanGetDevice(device, userId))
        {
            throw new DeviceNotFoundException();
        }

        return device;
    }

    public IEnumerable<Device> GetDevices(Guid roomId, Guid userId)
    {
        return _sqlDataContext.Devices
            .Join(
                _sqlDataContext.GetUsersRooms(userId).Where(room => room.Id == roomId),
                d => d.RoomId,
                r => r.Id,
                (d, r) => d
            )
            .ToList();
    }

    public void RemoveDevice(Guid deviceId, Guid userId)
    {
        var device = _sqlDataContext.Devices.FirstOrDefault(d => d.Id == deviceId);
        if (device == null)
        {
            throw new DeviceNotFoundException();
        }

        if (!CanGetDevice(device, userId))
        {
            throw new DeviceNotFoundException();
        }

        _sqlDataContext.Devices.Remove(device);
        _sqlDataContext.SaveChanges();
    }

    public void RenameDevice(Guid deviceId, string newName, Guid userId)
    {
        var device = GetDevice(deviceId, userId);
        device.Name = newName;

        _sqlDataContext.SaveChanges();
    }

    public void SetDeviceData(Guid deviceId, string data, Guid userId)
    {
        var device = GetDevice(deviceId, userId);
        device.Data = data;

        _sqlDataContext.SaveChanges();
    }

    private bool CanGetDevice(Device device, Guid userId)
    {
        return _roomService.UserCanAccessRoom(device.RoomId, userId);
    }
}
