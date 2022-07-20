using iHome.Models.Account.Rooms.Requests;
using iHome.Models.DataModels;
using iHome.Models.Requests;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace iHome.Models.Database
{
    public class AzureDatabaseApi : IDatabaseApi
    {
        private ApplicationdbContext db;

        public AzureDatabaseApi(string databaseServer, string databaseLogin, string databasePassword, string database)
        {
            db = new ApplicationdbContext(
            new ConnectionStringBuilder(databaseServer)
                .withLogin(databaseLogin)
                .withPassword(databasePassword)
                .withInitialCatalog(database)
                .build()
            );
        }

        public bool AddDevice(int id, string deviceId, string deviceName, int deviceType, string deviceData, int roomId)
        {
            db.Add(new TDevice
            {
                deviceId = deviceId,
                deviceName = deviceName,
                deviceType = deviceType,
                deviceData = deviceData,
                roomId = roomId
            });
            var deviceConfigurationToRemove = db.DevicesToConfigure?.Where(device => device.id == id).FirstOrDefault();
            if (deviceConfigurationToRemove != null)
            {
                db.DevicesToConfigure?.Remove(deviceConfigurationToRemove);
            }
            return db.SaveChanges() > 0;
        }

        public bool AddDevicesToConfigure(string deviceId, int deviceType, string ip)
        {
            db.DevicesToConfigure.Add(new TDeviceToConfigure
            {
                deviceId = deviceId,
                deviceType = deviceType,
                ipAddress = ip,
            });
            return db.SaveChanges() > 0;
        }

        public bool AddRoom(string roomName, string roomDescription, string uuid)
        {
            db.Rooms.Add(new TRoom()
            {
                roomName = roomName,
                roomDescription = roomDescription,
                roomImage = "",
                uuid = uuid,
            });
            if (db.SaveChanges() == 0)
            {
                return false;
            }
            int roomId = db.Rooms.OrderBy(room => room.roomId).Last().roomId;
            return ShareRoom(roomId, uuid);
        }

        public string GetDeviceData(string deviceId)
        {
            var deviceData = db.Devices
                    .Where(device => device.deviceId == deviceId)
                    .Select(device => device.deviceData)
                    .FirstOrDefault();
            return deviceData;
        }

        public List<Device> GetDevices(int roomId)
        {
            var devices = db.Devices.Where(device => device.roomId == roomId).ToList();
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
            var devicesToConfigure = db.DevicesToConfigure?.Where(device => device.ipAddress == ip).ToList();
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
                return db.Rooms
                        .Include(room => room.devices)
                        .Join(db.UsersRooms,
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
                        ).Where(room => room.uuid == uuid).ToList();
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
            var roomToRemove = db.Rooms.Where(room => room.roomId == roomId).FirstOrDefault();
            if (roomToRemove == null)
            {
                return false;
            }
            db.Rooms.Remove(roomToRemove);
            var usersRoomsToRemove = db.UsersRooms.Where(userRoom => userRoom.roomId == roomId).ToList();
            db.UsersRooms.RemoveRange(usersRoomsToRemove);
            if (db.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        public bool RenameDevice(string deviceId, string deviceName)
        {
            var deviceToChange = db.Devices.Where(device => device.deviceId == deviceId).FirstOrDefault();
            if (deviceToChange == null) return false;
            deviceToChange.deviceName = deviceName;
            db.Entry(deviceToChange).State = EntityState.Modified;
            return db.SaveChanges() > 0;
        }

        public bool SetDeviceData(string deviceId, string deviceData)
        {
            var device = db.Devices.FirstOrDefault(device => device.deviceId == deviceId);
            if (device == null) return false;
            device.deviceData = deviceData;
            db.Entry(device).State = EntityState.Modified;
            return db.SaveChanges() > 0;
        }

        public bool SetDeviceRoom(string deviceId, int roomId)
        {
            var deviceToChange = db.Devices.Where(device => device.deviceId == deviceId).FirstOrDefault();
            if (deviceToChange == null) return false;
            deviceToChange.roomId = roomId;
            db.Entry(deviceToChange).State = EntityState.Modified;
            return db.SaveChanges() > 0;
        }

        public bool ShareRoom(int roomId, string uuid)
        {
            db.UsersRooms?.Add(new()
            {
                uuid = uuid,
                roomId = roomId,
            });
            if (db.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
