using Microsoft.EntityFrameworkCore;
using iHome.Core.Models.Database;
using iHome.Core.Models.ApiRooms;
using iHome.Core.Logic.Database;
using iHome.Core.Helpers;
using iHome.Core.Models.Errors;

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
            var room = await _dbContext.Rooms.Where(room => room.RoomId == roomId).FirstOrDefaultAsync();
            if (room == null)
                throw new RoomNotFoundException();

            await _dbContext.Devices.AddAsync(new TDevice
            {
                DeviceId = deviceId,
                Name = deviceName,
                Type = deviceType,
                Data = deviceData,
                Room = room
            });
            var deviceConfigurationToRemove = _dbContext.DevicesToConfigure.Where(device => device.Id == id).FirstOrDefault();
            if (deviceConfigurationToRemove == null)
                throw new DeviceNotFoundException();
            
            _dbContext.DevicesToConfigure.Remove(deviceConfigurationToRemove);
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
            await AddUserRoomConstraint(room.Entity.RoomId, uuid);
        }

        public async Task<string> GetDeviceData(string deviceId, string uuid)
        {
            if (await CheckDeviceOwnership(deviceId, uuid))
            {
                var deviceData = await _dbContext.Devices
                        .Where(device => device.DeviceId == deviceId)
                        .Select(device => device.Data)
                        .FirstOrDefaultAsync();

                if (deviceData == null)
                    throw new DeviceNotFoundException();

                return deviceData;
            }
            return "{}";
        }

        public async Task<List<Device>> GetDevices(int roomId)
        {
            var devices = await _dbContext.Devices.Where(device => device.RoomId == roomId).ToListAsync();
            if (devices == null)
            {
                return new List<Device>();
            }
            return devices.GetDeviceList();
        }

        public async Task<List<TDeviceToConfigure>> GetDevicesToConfigure(string ip)
        {
            var devicesToConfigure = await _dbContext.DevicesToConfigure.Where(device => device.IpAddress == ip).ToListAsync();
            if (devicesToConfigure == null)
                throw new DeviceNotFoundException();
            return devicesToConfigure;
        }

        public Task<List<Room>> GetListOfRooms(string uuid)
        {
            var rooms = _dbContext.Rooms
                .Include(db => db.Devices)
                .Include(db => db.UsersRoom)
                .ToList()
                .Where(room => room.UsersRoom.Contains(uuid))
                .OrderBy(room => room.Name)
                .ToList()
                .ToRoomModelList(uuid);

            if (rooms == null)
                return Task.FromResult(new List<Room>());

            return Task.FromResult(rooms);
        }

        public async Task RemoveRoom(int roomId)
        {
            var roomToRemove = await _dbContext.Rooms.Where(room => room.RoomId == roomId).FirstOrDefaultAsync();
            if (roomToRemove == null)
                throw new RoomNotFoundException();
            var usersRoomsToRemove = roomToRemove.UsersRoom;
            if (usersRoomsToRemove == null)
                throw new UserRoomConstraintNotFoundException();

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
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task SetDeviceRoom(string deviceId, int roomId, string uuid)
        {
            if (await CheckDeviceOwnership(deviceId, uuid))
            {
                var deviceToChange = GetTDevice(deviceId);
                deviceToChange.RoomId = roomId;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task AddUserRoomConstraint(int roomId, string uuid)
        {
            if (!UserRoomConstraintFound(roomId, uuid) && uuid != "")
            {
                await _dbContext.UsersRooms.AddAsync(new() { RoomId = roomId, UserId = uuid});
                await _dbContext.SaveChangesAsync();
            }
        }
        private bool UserRoomConstraintFound(int roomId, string uuid)
        {
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

            if (users == null)
                throw new RoomNotFoundException();

            users.ForEach(userRoom => usersList.Add(userRoom.UserId));
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
            if (device == null)
                throw new DeviceNotFoundException();

            return device;
        }

        public async Task<List<string>> GetRoomUserIds(int roomId)
        {
            var userRooms = await _dbContext.UsersRooms
                .Where(userRoom => userRoom.RoomId == roomId)
                .ToListAsync();
            if (userRooms == null)
                throw new RoomNotFoundException();

            var uuids = new List<string>();
            userRooms.ForEach(userRoom => uuids.Add(userRoom.UserId));
            return uuids;
        }

        public async Task RemoveUserRoomConstraint(int roomId, string uuid, string masterUuid)
        {
            var room = await _dbContext.Rooms.Where(room => room.RoomId == roomId).FirstOrDefaultAsync();
            if (room == null)
                throw new RoomNotFoundException();

            if (room.UserId != masterUuid)
                throw new UnauthorizedAccessException();

            var userRoomToRemove = await _dbContext.UsersRooms
                .Where(userRoom => userRoom.RoomId == roomId && userRoom.UserId == uuid)
                .FirstOrDefaultAsync();
            if (userRoomToRemove == null)
                throw new UserRoomConstraintNotFoundException();

            _dbContext.UsersRooms.Remove(userRoomToRemove);
            await _dbContext.SaveChangesAsync();
        }
    }
}
