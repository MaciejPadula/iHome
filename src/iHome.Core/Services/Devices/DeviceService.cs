using iHome.Core.Exceptions;
using iHome.Core.Services.Rooms;
using iHome.Infrastructure.Firebase.Repositories;
using iHome.Infrastructure.SQL.Contexts;
using iHome.Infrastructure.SQL.Models;

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
            Data = "",
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

    public async Task<Device> GetDevice(Guid deviceId, string userId)
    {
        var device = await _sqlDataContext.Devices.FirstOrDefaultAsync(d => d.Id == deviceId);
        if (device == null || !await CanGetDevice(device, userId))
        {
            throw new DeviceNotFoundException();
        }

        return device;
    }

    public async Task<IEnumerable<Device>> GetDevices(Guid roomId, string userId)
    {
        var userRooms = await _roomService.GetRoomsWithDevices(userId);

        var selectedRoom = userRooms.FirstOrDefault(room => room.Id == roomId);
        if (selectedRoom == null) throw new RoomNotFoundException();

        return selectedRoom.Devices ?? Enumerable.Empty<Device>();
    }

    public async Task RemoveDevice(Guid deviceId, string userId)
    {
        var device = await _sqlDataContext.Devices.FirstOrDefaultAsync(d => d.Id == deviceId);
        if (device == null)
        {
            throw new DeviceNotFoundException();
        }

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
