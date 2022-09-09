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

        public void AddDevice(int id, string deviceId, string deviceName, int deviceType, string deviceData, int roomId)
        {
            _dbContext.Devices.Add(new TDevice
            {
                DeviceId = deviceId,
                Name = deviceName,
                Type = deviceType,
                Data = deviceData,
                Room = _dbContext.Rooms.Where(room => room.RoomId == roomId).FirstOrDefault(new TRoom())
            });
            var deviceConfigurationToRemove = _dbContext.DevicesToConfigure?.Where(device => device.Id == id).FirstOrDefault();
            if (deviceConfigurationToRemove != null)
            {
                _dbContext.DevicesToConfigure?.Remove(deviceConfigurationToRemove);
            }
            _dbContext.SaveChanges();
        }

        public void AddRoom(string roomName, string roomDescription, string uuid)
        {
            var room = _dbContext.Rooms.Add(new TRoom()
            {
                Name = roomName,
                Description = roomDescription,
                UserId = uuid,
            });
            _dbContext.SaveChanges();
            ShareRoom(room.Entity.RoomId, uuid);
        }

        public string GetDeviceData(string deviceId, string uuid)
        {
            if (CheckDeviceOwnership(deviceId, uuid))
            {
                var deviceData = _dbContext?.Devices?
                        .Where(device => device.DeviceId == deviceId)
                        .Select(device => device.Data)
                        .FirstOrDefault();
                if (deviceData != null)
                {
                    return deviceData;
                }
            }
            return "{}";
        }

        public List<Device> GetDevices(int roomId)
        {
            var devices = _dbContext.Devices.Where(device => device.RoomId == roomId).ToList();
            if (devices != null)
            {
                return devices.GetDeviceList();
            }
            return new();
        }

        public List<TDeviceToConfigure> GetDevicesToConfigure(string ip)
        {
            var devicesToConfigure = _dbContext.DevicesToConfigure?.Where(device => device.IpAddress == ip).ToList();
            if (devicesToConfigure != null)
            {
                return devicesToConfigure;
            }
            return new();
        }

        public List<Room> GetListOfRooms(string uuid)
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
                return rooms;
            }
            return new List<Room>();
        }

        public void RemoveRoom(int roomId)
        {
            var roomToRemove = _dbContext.Rooms.Where(room => room.RoomId == roomId).FirstOrDefault();
            if (roomToRemove == null)
                throw new Exception("No room found");

            var usersRoomsToRemove = roomToRemove.UsersRoom;
            _dbContext.Rooms.Remove(roomToRemove);
            _dbContext.UsersRooms.RemoveRange(usersRoomsToRemove);
            _dbContext.SaveChanges();
        }

        public void RenameDevice(string deviceId, string deviceName, string uuid)
        {
            if (CheckDeviceOwnership(deviceId, uuid))
            {
                var deviceToChange = GetTDevice(deviceId);
                deviceToChange.Name = deviceName;
                _dbContext.Entry(deviceToChange).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
        }

        public void SetDeviceData(string deviceId, string deviceData, string uuid)
        {
            if (CheckDeviceOwnership(deviceId, uuid))
            {
                var device = GetTDevice(deviceId);
                device.Data = deviceData;
                _dbContext.Entry(device).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
        }

        public void SetDeviceRoom(string deviceId, int roomId, string uuid)
        {
            if (CheckDeviceOwnership(deviceId, uuid))
            {
                var deviceToChange = GetTDevice(deviceId);
                deviceToChange.RoomId = roomId;
                _dbContext.Entry(deviceToChange).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
        }

        public void ShareRoom(int roomId, string uuid)
        {
            if (!UserRoomConstraintFound(roomId, uuid) && uuid != "")
            {
                _dbContext.UsersRooms.Add(new() { RoomId = roomId, UserId = uuid});
                _dbContext.SaveChanges();
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
        private List<string> GetOwnersOfDevice(string deviceId)
        {
            var roomId = GetDeviceRoomId(deviceId);
            var users = _dbContext?.UsersRooms?.Where(userRoom => userRoom.RoomId == roomId).ToList();
            List<string> usersList = new List<string>();

            if (users != null)
            {
                users.ForEach(userRoom => usersList.Add(userRoom.UserId));
            }

            return usersList;
        }
        private bool CheckDeviceOwnership(string deviceId, string uuid)
        {
            var checkedOwnership = false;
            var owners = GetOwnersOfDevice(deviceId);
            owners.ForEach(user =>
            {
                if (user.Equals(uuid))
                {
                    checkedOwnership = true;
                }
            });
            return checkedOwnership;
        }

        public int GetDeviceRoomId(string deviceId)
        {
            return GetTDevice(deviceId).RoomId;
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

        public List<string> GetRoomUserIds(int roomId)
        {
            var userRooms = _dbContext?.UsersRooms?
                .Where(userRoom => userRoom.RoomId == roomId)
                .ToList();
            var uuids = new List<string>();

            if (userRooms != null)
            {
                userRooms.ForEach(userRoom => uuids.Add(userRoom.UserId));
            }

            return uuids;
        }

        public void RemoveRoomShare(int roomId, string uuid, string masterUuid)
        {
            var room = _dbContext.Rooms.Where(room => room.RoomId == roomId).FirstOrDefault();
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
