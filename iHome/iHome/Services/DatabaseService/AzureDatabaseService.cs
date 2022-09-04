using iHome.Models.DataModels;
using Microsoft.EntityFrameworkCore;
using iHome.Models.Database;
using iHome.Logic.Database;
using iHome.Logic.Utils;

namespace iHome.Services.DatabaseService
{
    public class AzureDatabaseService : IDatabaseService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AzureDatabaseService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public bool AddDevice(int id, string deviceId, string deviceName, int deviceType, string deviceData, int roomId)
        {
            _applicationDbContext.Add(new TDevice
            {
                DeviceId = deviceId,
                DeviceName = deviceName,
                DeviceType = deviceType,
                DeviceData = deviceData,
                Room = _applicationDbContext.Rooms.Where(room => room.RoomId == roomId).FirstOrDefault()
            });
            var deviceConfigurationToRemove = _applicationDbContext.DevicesToConfigure?.Where(device => device.Id == id).FirstOrDefault();
            if (deviceConfigurationToRemove != null)
            {
                _applicationDbContext.DevicesToConfigure?.Remove(deviceConfigurationToRemove);
            }
            return _applicationDbContext.SaveChanges() > 0;
        }

        public bool AddDevicesToConfigure(string deviceId, int deviceType, string ip)
        {
            _applicationDbContext?.DevicesToConfigure?.Add(new TDeviceToConfigure
            {
                DeviceId = deviceId,
                DeviceType = deviceType,
                IpAddress = ip,
            });
            return _applicationDbContext?.SaveChanges() > 0;
        }

        public bool AddRoom(string roomName, string roomDescription, string uuid)
        {
            _applicationDbContext?.Rooms?.Add(new TRoom()
            {
                RoomName = roomName,
                RoomDescription = roomDescription,
                RoomImage = "",
                Uuid = uuid,
            });
            if (_applicationDbContext?.SaveChanges() == 0)
            {
                return false;
            }
            int roomId = 0;
            if (_applicationDbContext?.Rooms != null)
            {
                roomId = _applicationDbContext.Rooms.OrderBy(room => room.RoomId).Last().RoomId;
            }
            
            return ShareRoom(roomId, uuid);
        }

        public string GetDeviceData(string deviceId, string uuid)
        {
            if (CheckDeviceOwnership(deviceId, uuid)) 
            { 
                var deviceData = _applicationDbContext?.Devices?
                        .Where(device => device.DeviceId == deviceId)
                        .Select(device => device.DeviceData)
                        .FirstOrDefault();
                if(deviceData != null)
                {
                    return deviceData;
                }
            }
            return "{}";
        }

        public List<Device> GetDevices(int roomId)
        {
            var devices = _applicationDbContext.Devices.Where(device => device.RoomId == roomId).ToList();
            if (devices != null)
            {
                return DataModelsConversionUtils.ListOfDevicesFromListOfTDevices(devices);
            }
            return new List<Device>();
        }

        public List<TDeviceToConfigure>? GetDevicesToConfigure(string ip)
        {
            var devicesToConfigure = _applicationDbContext.DevicesToConfigure?.Where(device => device.IpAddress == ip).ToList();
            if (devicesToConfigure != null)
            {
                return devicesToConfigure;
            }
            return null;
        }

        public List<Room>? GetListOfRooms(string uuid)
        {
            var rooms = _applicationDbContext.Rooms
                        .Join(_applicationDbContext.UsersRooms,
                            room => room.RoomId,
                            userRoom => userRoom.RoomId,
                            (room, userRoom) => new Room
                            {
                                Id = room.RoomId,
                                Description = room.RoomDescription,
                                Image = room.RoomImage,
                                Name = room.RoomName,
                                Devices = DataModelsConversionUtils.ListOfDevicesFromListOfTDevices(room.Devices),
                                Uuid = userRoom.Uuid,
                                OwnerUuid = room.Uuid
                            }
                        ).Where(room => room.Uuid == uuid)
                        .OrderBy(room => room.Name)
                        .ToList();
            if(rooms != null)
            {
                return rooms;
            }
            return new List<Room>();
        }

        public bool RemoveRoom(int roomId)
        {
            var roomToRemove = _applicationDbContext.Rooms.Where(room => room.RoomId == roomId).FirstOrDefault();
            if (roomToRemove == null)
            {
                return false;
            }
            _applicationDbContext.Rooms.Remove(roomToRemove);
            var usersRoomsToRemove = _applicationDbContext.UsersRooms.Where(userRoom => userRoom.RoomId == roomId).ToList();
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
                deviceToChange.DeviceName = deviceName;
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
                device.DeviceData = deviceData;
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
                deviceToChange.RoomId = roomId;
                _applicationDbContext.Entry(deviceToChange).State = EntityState.Modified;
                return _applicationDbContext.SaveChanges() > 0;
            }
            return false;
        }

        public bool ShareRoom(int roomId, string uuid)
        {
            if (!UserRoomConstraintFound(roomId, uuid) && uuid != "")
            {
                _applicationDbContext.UsersRooms?.Add(new()
                {
                    Uuid = uuid,
                    RoomId = roomId,
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
            if(_applicationDbContext.UsersRooms == null)
                return false;
            return _applicationDbContext.UsersRooms
                .Where(userRoom => userRoom.RoomId == roomId && userRoom.Uuid == uuid)
                .ToList()
                .Any();
        }
        private List<string> GetOwnersOfDevice(string deviceId)
        {
            var roomId = GetDeviceRoomId(deviceId);
            var users = _applicationDbContext?.UsersRooms?.Where(userRoom => userRoom.RoomId == roomId).ToList();
            List<string> usersList = new List<string>();

            if(users != null)
            {
                users.ForEach(userRoom => usersList.Add(userRoom.Uuid));
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

        private Device GetDevice(string deviceId)
        {
            return DataModelsConversionUtils.DeviceFromTDevice(GetTDevice(deviceId));
        }

        private TDevice GetTDevice(string deviceId)
        {
            var device = _applicationDbContext.Devices.Where(dev => dev.DeviceId == deviceId).FirstOrDefault();
            if (device != null)
            {
                return device;
            }
            return new TDevice();
        }

        public List<string> GetRoomUserIds(int roomId)
        {
            var userRooms = _applicationDbContext?.UsersRooms?
                .Where(userRoom => userRoom.RoomId == roomId)
                .ToList();
            var uuids = new List<string>();

            if(userRooms != null)
            {
                userRooms.ForEach(userRoom => uuids.Add(userRoom.Uuid));
            }
            
            return uuids;
        }

        public bool RemoveRoomShare(int roomId, string uuid, string masterUuid)
        {
            var room = _applicationDbContext?.Rooms?.Where(room => room.RoomId == roomId).FirstOrDefault();
            if(room == null) { return false; }
            if(room.Uuid != masterUuid) { return false; }

            var toRemove = _applicationDbContext?.UsersRooms?
                .Where(userRoom => userRoom.RoomId == roomId && userRoom.Uuid == uuid)
                .FirstOrDefault();
            if(toRemove != null)
            {
                _applicationDbContext?.UsersRooms?.Remove(toRemove);
            }
            return _applicationDbContext?.SaveChanges() >= 1;
        }

        public List<TBills> GetUserBills(string uuid)
        {
            var bills = _applicationDbContext?.Bills?.Where(bill => bill.Uuid == uuid).ToList();
            if(bills != null)
                return bills;
            return new List<TBills>();
        }
    }
}
