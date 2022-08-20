using iHome.Models.DataModels;
using Microsoft.EntityFrameworkCore;
using iHome.Models.Database;
using iHome.Logic.Database;
using iHome.Logic.Utils;

namespace iHome.Services.DatabaseService
{
    public class AzureDatabaseService : IDatabaseService
    {
        private ApplicationDbContext _applicationDbContext;

        public AzureDatabaseService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public bool AddDevice(int id, string deviceId, string deviceName, int deviceType, string deviceData, int roomId)
        {
            _applicationDbContext.Add(new TDevice
            {
                deviceId = deviceId,
                deviceName = deviceName,
                deviceType = deviceType,
                deviceData = deviceData,
                roomId = roomId
            });
            var deviceConfigurationToRemove = _applicationDbContext.DevicesToConfigure?.Where(device => device.id == id).FirstOrDefault();
            if (deviceConfigurationToRemove != null)
            {
                _applicationDbContext.DevicesToConfigure?.Remove(deviceConfigurationToRemove);
            }
            return _applicationDbContext.SaveChanges() > 0;
        }

        public bool AddDevicesToConfigure(string deviceId, int deviceType, string ip)
        {
            _applicationDbContext.DevicesToConfigure.Add(new TDeviceToConfigure
            {
                deviceId = deviceId,
                deviceType = deviceType,
                ipAddress = ip,
            });
            return _applicationDbContext.SaveChanges() > 0;
        }

        public bool AddRoom(string roomName, string roomDescription, string uuid)
        {
            _applicationDbContext.Rooms.Add(new TRoom()
            {
                roomName = roomName,
                roomDescription = roomDescription,
                roomImage = "",
                uuid = uuid,
            });
            if (_applicationDbContext.SaveChanges() == 0)
            {
                return false;
            }
            int roomId = _applicationDbContext.Rooms.OrderBy(room => room.roomId).Last().roomId;
            return ShareRoom(roomId, uuid);
        }

        public string GetDeviceData(string deviceId, string uuid)
        {
            if (CheckDeviceOwnership(deviceId, uuid)) 
            { 
                var deviceData = _applicationDbContext.Devices
                        .Where(device => device.deviceId == deviceId)
                        .Select(device => device.deviceData)
                        .FirstOrDefault();
                return deviceData;
            }
            return "{}";
        }

        public List<Device> GetDevices(int roomId)
        {
            var devices = _applicationDbContext.Devices.Where(device => device.roomId == roomId).ToList();
            if (devices != null)
            {
                return DataModelsConversionUtils.ListOfDevicesFromListOfTDevices(devices);
            }
            return null;
        }

        public int GetDevicesCount(string uuid)
        {
            int devicesCount = 0;
            GetListOfRooms(uuid)?.ForEach(room =>
            {
                devicesCount += room.devices.Count;
            });
            return devicesCount;
        }

        public List<TDeviceToConfigure>? GetDevicesToConfigure(string ip)
        {
            var devicesToConfigure = _applicationDbContext.DevicesToConfigure?.Where(device => device.ipAddress == ip).ToList();
            if (devicesToConfigure != null)
            {
                return devicesToConfigure;
            }
            return null;
        }

        public List<Room>? GetListOfRooms(string uuid)
        {
            try
            {
                return _applicationDbContext.Rooms
                        .Include(room => room.devices)
                        .Join(_applicationDbContext.UsersRooms,
                            room => room.roomId,
                            userRoom => userRoom.roomId,
                            (room, userRoom) => new Room
                            {
                                roomId = room.roomId,
                                roomDescription = room.roomDescription,
                                roomImage = room.roomImage,
                                roomName = room.roomName,
                                devices = DataModelsConversionUtils.ListOfDevicesFromListOfTDevices(room.devices),
                                uuid = userRoom.uuid,
                                masterUuid = room.uuid
                            }
                        ).Where(room => room.uuid == uuid)
                        .OrderBy(room => room.roomName)
                        .ToList();
            }
            catch { }
            return null;
        }

        public int GetRoomsCount(string uuid)
        {
            int roomsCount = GetListOfRooms(uuid).Count();
            return roomsCount;
        }

        public bool RemoveRoom(int roomId)
        {
            var roomToRemove = _applicationDbContext.Rooms.Where(room => room.roomId == roomId).FirstOrDefault();
            if (roomToRemove == null)
            {
                return false;
            }
            _applicationDbContext.Rooms.Remove(roomToRemove);
            var usersRoomsToRemove = _applicationDbContext.UsersRooms.Where(userRoom => userRoom.roomId == roomId).ToList();
            _applicationDbContext.UsersRooms.RemoveRange(usersRoomsToRemove);
            if (_applicationDbContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        public bool RenameDevice(string deviceId, string deviceName, string uuid)
        {
            if(CheckDeviceOwnership(deviceId, uuid))
            {
                var deviceToChange = GetTDevice(deviceId);
                if (deviceToChange == null) return false;
                deviceToChange.deviceName = deviceName;
                _applicationDbContext.Entry(deviceToChange).State = EntityState.Modified;
                return _applicationDbContext.SaveChanges() > 0;
            }
            return false;
        }

        public bool SetDeviceData(string deviceId, string deviceData, string uuid)
        {
            if (CheckDeviceOwnership(deviceId, uuid))
            {
                var device = GetTDevice(deviceId);
                if (device == null) return false;
                device.deviceData = deviceData;
                _applicationDbContext.Entry(device).State = EntityState.Modified;
                return _applicationDbContext.SaveChanges() > 0;
            }
            return false;
        }

        public bool SetDeviceRoom(string deviceId, int roomId, string uuid)
        {
            if (CheckDeviceOwnership(deviceId, uuid))
            {
                var deviceToChange = GetTDevice(deviceId);
                if (deviceToChange == null) return false;
                deviceToChange.roomId = roomId;
                _applicationDbContext.Entry(deviceToChange).State = EntityState.Modified;
                return _applicationDbContext.SaveChanges() > 0;
            }
            return false;
        }

        public bool ShareRoom(int roomId, string uuid)
        {
            if (!UserRoomConstraintFound(roomId, uuid))
            {
                _applicationDbContext.UsersRooms?.Add(new()
                {
                    uuid = uuid,
                    roomId = roomId,
                });
                if (_applicationDbContext.SaveChanges() > 0)
                {
                    return true;
                }
            }
            
            return false;
        }
        private bool UserRoomConstraintFound(int roomId, string uuid)
        {
            return _applicationDbContext.UsersRooms?.Where(userRoom => userRoom.roomId == roomId && userRoom.uuid == uuid).ToList().Count > 0;
        }
        private List<string> GetOwnersOfDevice(string deviceId)
        {
            var roomId = GetDeviceRoomId(deviceId);
            var users = _applicationDbContext.UsersRooms.Where(userRoom => userRoom.roomId == roomId).ToList();
            List<string> usersList = new List<string>();

            users.ForEach(userRoom => usersList.Add(userRoom.uuid));

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
            return GetTDevice(deviceId).roomId;
        }

        private Device GetDevice(string deviceId)
        {
            return DataModelsConversionUtils.DeviceFromTDevice(GetTDevice(deviceId));
        }

        private TDevice GetTDevice(string deviceId)
        {
            var rooms = _applicationDbContext.Rooms
                        .Include(room => room.devices)
                        .ToList();
            TDevice dev = new TDevice();
            rooms.ForEach(room =>
            {
                room.devices.ForEach(device =>
                {
                    if (device.deviceId == deviceId)
                    {
                        dev = device;
                    }
                });
            });
            return dev;
        }

        public List<string> GetRoomUserIds(int roomId)
        {
            var userRooms = _applicationDbContext.UsersRooms
                .Where(userRoom => userRoom.roomId == roomId)
                .ToList();
            var uuids = new List<string>();
            userRooms.ForEach(userRoom => uuids.Add(userRoom.uuid));
            return uuids;
        }

        public bool RemoveRoomShare(int roomId, string uuid, string masterUuid)
        {
            var room = _applicationDbContext.Rooms.Where(room => room.roomId == roomId).FirstOrDefault();
            if(room.uuid != masterUuid) { return false; }

            var toRemove = _applicationDbContext.UsersRooms
                .Where(userRoom => userRoom.roomId == roomId && userRoom.uuid == uuid)
                .FirstOrDefault();
            if(toRemove != null)
            {
                _applicationDbContext.UsersRooms.Remove(toRemove);
            }
            return _applicationDbContext.SaveChanges() >= 1;

        }
    }
}
