using iHome.Models.Database;
using iHome.Models.DataModels;

namespace iHome.Services.DatabaseService
{
    public interface IDatabaseService
    {
        List<Room>? GetListOfRooms(string uuid);
        bool AddRoom(string roomName, string roomDescription, string uuid);
        bool RemoveRoom(int roomId);
        bool ShareRoom(int roomId, string uuid);
        List<Device> GetDevices(int roomId);
        bool AddDevice(int id, string deviceId, string deviceName, int deviceType, string deviceData, int roomId);
        bool RenameDevice(string deviceId, string deviceName, string uuid);
        string GetDeviceData(string deviceId, string uuid);
        bool SetDeviceData(string deviceId, string deviceData, string uuid);
        bool SetDeviceRoom(string deviceId, int roomId, string uuid);
        List<TDeviceToConfigure>? GetDevicesToConfigure(string ip);
        bool AddDevicesToConfigure(string deviceId, int deviceType, string ip);
        List<string> GetRoomUserIds(int roomId);
        int GetDeviceRoomId(string deviceId);
        bool RemoveRoomShare(int roomId, string uuid, string masterUuid);

        List<TBills> GetUserBills(string uuid); 
    }
}
