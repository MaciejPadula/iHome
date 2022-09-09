using iHome.Core.Models.ApiRooms;
using iHome.Core.Models.Database;

namespace iHome.Core.Services.DatabaseService
{
    public interface IDatabaseService
    {
        void AddRoom(string roomName, string roomDescription, string uuid);
        List<Room> GetListOfRooms(string uuid);
        void RemoveRoom(int roomId);

        void ShareRoom(int roomId, string uuid);
        void RemoveRoomShare(int roomId, string uuid, string masterUuid);

        void AddDevice(int id, string deviceId, string deviceName, int deviceType, string deviceData, int roomId);        
        List<Device> GetDevices(int roomId);
        void RenameDevice(string deviceId, string deviceName, string uuid);
        void SetDeviceData(string deviceId, string deviceData, string uuid);
        string GetDeviceData(string deviceId, string uuid);
        
        void SetDeviceRoom(string deviceId, int roomId, string uuid);
        List<TDeviceToConfigure> GetDevicesToConfigure(string ip);
        List<string> GetRoomUserIds(int roomId);
        int GetDeviceRoomId(string deviceId);
        
    }
}
