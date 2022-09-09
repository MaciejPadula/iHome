using Microsoft.EntityFrameworkCore;
using iHome.Core.Models.Database;
using iHome.Core.Models.ApiRooms;
using iHome.Core.Logic.Database;
using iHome.Core.Helpers;

namespace iHome.Core.Services.DatabaseService
{
    public class AzureDatabaseService : IDatabaseService
    {
        private readonly AppDbContext _dbContext;

        public AzureDatabaseService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.Database.EnsureCreated();
        }

        public async Task AddDevice(int id, string deviceId, string deviceName, int deviceType, string deviceData, int roomId)
        {
            await _dbContext.Devices.AddAsync(new TDevice
            {
                DeviceId = deviceId,
                Name = deviceName,
                Type = deviceType,
                Data = deviceData,
                Room = _dbContext.Rooms.Where(room => room.RoomId == roomId).FirstOrDefault(new TRoom())
            });
            var deviceConfigurationToRemove = _dbContext.DevicesToConfigure.Where(device => device.Id == id).FirstOrDefault();
            if (deviceConfigurationToRemove != null)
            {
                _dbContext.DevicesToConfigure?.Remove(deviceConfigurationToRemove);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRoom(string roomName, string roomDescription, string uuid)
        {
            var room = await _dbContext.Rooms.AddAsync(new TRoom()
            {
                Name = roomName,
                Description = roomDescription,
                UserId = uuid,
            });
            await _dbContext.SaveChangesAsync();
            await ShareRoom(room.Entity.RoomId, uuid);
        }

        public async Task<string> GetDeviceData(string deviceId, string uuid)
        {
            if (await CheckDeviceOwnership(deviceId, uuid))
            {
                var deviceData = await _dbContext.Devices
                        .Where(device => device.DeviceId == deviceId)
                        .Select(device => device.Data)
                        .FirstOrDefaultAsync();
                if (deviceData != null)
                {
                    return deviceData;
                }
            }
            return "{}";
        }

        public async Task<List<Device>> GetDevices(int roomId)
        {
            var devices = await _dbContext.Devices.Where(device => device.RoomId == roomId).ToListAsync();
            if (devices != null)
            {
                return devices.GetDeviceList();
            }
            return new();
        }

        public async Task<List<TDeviceToConfigure>> GetDevicesToConfigure(string ip)
        {
            var devicesToConfigure = await _dbContext.DevicesToConfigure.Where(device => device.IpAddress == ip).ToListAsync();
            if (devicesToConfigure != null)
            {
                return devicesToConfigure;
            }
            return new();
        }

        public Task<List<Room>> GetListOfRooms(string uuid)
        {
            var rooms = _dbContext.Rooms
                .Include(db => db.Devices)
                .Include(db => db.UsersRoom)
                .ToList()
                .Where(room => room.UsersRoom.Contains(uuid))
                .ToList()
                .ToRoomModelList(uuid);

            if (rooms != null)
            {
                return Task.FromResult(rooms);
            }
            return Task.FromResult(new List<Room>());
        }

        public async Task RemoveRoom(int roomId)
        {
            var roomToRemove = await _dbContext.Rooms.Where(room => room.RoomId == roomId).FirstOrDefaultAsync();
            if (roomToRemove == null)
                throw new Exception("No room found");

            var usersRoomsToRemove = roomToRemove.UsersRoom;
            _dbContext.Rooms.Remove(roomToRemove);
            _dbContext.UsersRooms.RemoveRange(usersRoomsToRemove);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RenameDevice(string deviceId, string deviceName, string uuid)
        {
            if (await CheckDeviceOwnership(deviceId, uuid))
            {
                var deviceToChange = GetTDevice(deviceId);
                deviceToChange.Name = deviceName;
                _dbContext.Entry(deviceToChange).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task SetDeviceData(string deviceId, string deviceData, string uuid)
        {
            if (await CheckDeviceOwnership(deviceId, uuid))
            {
                var device = GetTDevice(deviceId);
                device.Data = deviceData;
                _dbContext.Entry(device).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task SetDeviceRoom(string deviceId, int roomId, string uuid)
        {
            if (await CheckDeviceOwnership(deviceId, uuid))
            {
                var deviceToChange = GetTDevice(deviceId);
                deviceToChange.RoomId = roomId;
                _dbContext.Entry(deviceToChange).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task ShareRoom(int roomId, string uuid)
        {
            if (!UserRoomConstraintFound(roomId, uuid) && uuid != "")
            {
                await _dbContext.UsersRooms.AddAsync(new() { RoomId = roomId, UserId = uuid});
                await _dbContext.SaveChangesAsync();
            }
        }
        private bool UserRoomConstraintFound(int roomId, string uuid)
        {
            if (_dbContext.UsersRooms == null)
                return false;
            return _dbContext.UsersRooms
                .Where(userRoom => userRoom.RoomId == roomId && userRoom.UserId == uuid)
                .ToList()
                .Any();
        }
        private async Task<List<string>> GetOwnersOfDevice(string deviceId)
        {
            var roomId = await GetDeviceRoomId(deviceId);
            var users = _dbContext.UsersRooms.Where(userRoom => userRoom.RoomId == roomId).ToList();
            List<string> usersList = new List<string>();

            if (users != null)
            {
                users.ForEach(userRoom => usersList.Add(userRoom.UserId));
            }

            return usersList;
        }
        private async Task<bool> CheckDeviceOwnership(string deviceId, string uuid)
        {
            var checkedOwnership = false;
            var owners = await GetOwnersOfDevice(deviceId);
            owners.ForEach(user =>
            {
                if (user.Equals(uuid))
                {
                    checkedOwnership = true;
                }
            });
            return checkedOwnership;
        }

        public Task<int> GetDeviceRoomId(string deviceId)
        {
            return Task.FromResult(GetTDevice(deviceId).RoomId);
        }

        private TDevice GetTDevice(string deviceId)
        {
            var device = _dbContext.Devices.Where(dev => dev.DeviceId == deviceId).FirstOrDefault();
            if (device != null)
            {
                return device;
            }
            return new TDevice();
        }

        public async Task<List<string>> GetRoomUserIds(int roomId)
        {
            var userRooms = await _dbContext.UsersRooms
                .Where(userRoom => userRoom.RoomId == roomId)
                .ToListAsync();
            var uuids = new List<string>();

            if (userRooms != null)
            {
                userRooms.ForEach(userRoom => uuids.Add(userRoom.UserId));
            }

            return uuids;
        }

        public async Task RemoveRoomShare(int roomId, string uuid, string masterUuid)
        {
            var room = await _dbContext.Rooms.Where(room => room.RoomId == roomId).FirstOrDefaultAsync();
            if (room == null)
                throw new Exception("No room uses this ID");

            if (room.UserId != masterUuid) 
                throw new Exception("Unauthorized");

            var toRemove = _dbContext.UsersRooms
                .Where(userRoom => userRoom.RoomId == roomId && userRoom.UserId == uuid)
                .FirstOrDefault();
            if (toRemove == null)
                throw new Exception("No Constraint");

            _dbContext.UsersRooms.Remove(toRemove);
            _dbContext.SaveChanges();
        }
    }
}
