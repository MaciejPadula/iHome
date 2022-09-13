using iHome.Core.Helpers;
using iHome.Core.Logic.Database;
using iHome.Core.Middleware.Exceptions;
using iHome.Core.Models.ApiRooms;
using iHome.Core.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Services.DevicesService
{
    public class DevicesService: IDevicesService
    {
        private readonly AppDbContext _dbContext;

        public DevicesService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddDevice(int id, string deviceId, string deviceName, int deviceType, string deviceData, int roomId)
        {
            var room = await _dbContext.Rooms.Where(room => room.RoomId == roomId).FirstOrDefaultAsync();
            if (room == null)
                throw new RoomNotFoundException();

            await _dbContext.Devices.AddAsync(new TDevice(deviceId, deviceName, deviceType, deviceData, roomId, room));
            var deviceConfigurationToRemove = _dbContext.DevicesToConfigure.Where(device => device.Id == id).FirstOrDefault();
            if (deviceConfigurationToRemove == null)
                throw new DeviceNotFoundException();

            _dbContext.DevicesToConfigure.Remove(deviceConfigurationToRemove);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> GetDeviceData(string deviceId, string uuid)
        {
            if (!await CheckDeviceOwnership(deviceId, uuid))
                return "{}";
            var deviceData = await _dbContext.Devices
                        .Where(device => device.DeviceId == deviceId)
                        .Select(device => device.Data)
                        .FirstOrDefaultAsync();
            if (deviceData == null)
                throw new DeviceNotFoundException();

            return deviceData;
        }

        public Task<int> GetDeviceRoomId(string deviceId)
        {
            return Task.FromResult(GetTDevice(deviceId).RoomId);
        }

        public async Task<List<TDeviceToConfigure>> GetDevicesToConfigure(string ip)
        {
            var devicesToConfigure = await _dbContext.DevicesToConfigure.Where(device => device.IpAddress == ip).ToListAsync();
            if (devicesToConfigure == null)
                throw new DeviceNotFoundException();

            return devicesToConfigure;
        }

        public async Task RenameDevice(string deviceId, string deviceName, string uuid)
        {
            if (!await CheckDeviceOwnership(deviceId, uuid))
                return;
            var deviceToChange = GetTDevice(deviceId);

            deviceToChange.Name = deviceName;
            await _dbContext.SaveChangesAsync();
        }

        public async Task SetDeviceData(string deviceId, string deviceData, string uuid)
        {
            if (!await CheckDeviceOwnership(deviceId, uuid))
                return;
            var device = GetTDevice(deviceId);

            device.Data = deviceData;
            await _dbContext.SaveChangesAsync();
        }

        public async Task SetDeviceRoom(string deviceId, int roomId, string uuid)
        {
            if (!await CheckDeviceOwnership(deviceId, uuid))
                return;
            var deviceToChange = GetTDevice(deviceId);

            deviceToChange.RoomId = roomId;
            await _dbContext.SaveChangesAsync();
        }

        private TDevice GetTDevice(string deviceId)
        {
            var device = _dbContext.Devices.Where(dev => dev.DeviceId == deviceId).FirstOrDefault();
            if (device == null)
                throw new DeviceNotFoundException();

            return device;
        }

        private async Task<bool> CheckDeviceOwnership(string deviceId, string uuid)
        {
            var roomId = await GetDeviceRoomId(deviceId);
            var user = await _dbContext.UsersRooms.Where(userRoom => userRoom.UserId == uuid).FirstOrDefaultAsync();

            return user != null;
        }
    }
}
