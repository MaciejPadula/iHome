using iHome.Core.Exceptions;
using iHome.Core.Repositories;
using iHome.Core.Services.Rooms;
using iHome.Devices.Contract.Models;

namespace iHome.Core.Services.Devices;

public class DeviceService : IDeviceService
{
    private readonly IRoomService _roomService;
    private readonly DevicesDataContext _deviceDataContext;

    public DeviceService(IRoomService roomService, DevicesDataContext deviceDataContext)
    {
        _roomService = roomService;
        _deviceDataContext = deviceDataContext;
    }

    public Guid AddDevice(string name, string macAddress, DeviceType type, string hubId, Guid roomId, Guid userId)
    {
        if (!_roomService.UserCanAccessRoom(roomId, userId))
        {
            throw new RoomNotFoundException();
        }

        var deviceId = _deviceDataContext.Devices.Add(new Device
        {
            Name = name,
            Type = type,
            Data = "",
            HubId = hubId,
            RoomId = roomId,
            MacAddress = macAddress
        }).Entity.Id;

        _deviceDataContext.SaveChanges();
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

        _deviceDataContext.SaveChanges();
    }

    public Device GetDevice(Guid deviceId, Guid userId)
    {
        var device = _deviceDataContext.Devices.FirstOrDefault(d => d.Id == deviceId);
        if (device == null)
        {
            throw new DeviceNotFoundException();
        }

        if (!CanGetDevice(device, userId))
        {
            throw new DeviceNotFoundException();
        }

        return device;
    }

    public IEnumerable<Device> GetDevices(Guid userId)
    {
        return _deviceDataContext.Devices
            .Join(
                _roomService.GetRooms(userId),
                d => d.RoomId,
                u => u.Id,
                (d, u) => d
            )
            .ToList();
    }

    public void RemoveDevice(Guid deviceId, Guid userId)
    {
        var device = _deviceDataContext.Devices.FirstOrDefault(d => d.Id == deviceId);
        if (device == null)
        {
            throw new DeviceNotFoundException();
        }

        if (!CanGetDevice(device, userId))
        {
            throw new DeviceNotFoundException();
        }

        _deviceDataContext.Devices.Remove(device);
        _deviceDataContext.SaveChanges();
    }

    public void RenameDevice(Guid deviceId, string newName, Guid userId)
    {
        var device = GetDevice(deviceId, userId);
        device.Name = newName;

        _deviceDataContext.SaveChanges();
    }

    public void SetDeviceData(Guid deviceId, string data, Guid userId)
    {
        var device = GetDevice(deviceId, userId);
        device.Data = data;

        _deviceDataContext.SaveChanges();
    }

    private bool CanGetDevice(Device device, Guid userId)
    {
        return _roomService.UserCanAccessRoom(device.RoomId, userId);
    }
}
