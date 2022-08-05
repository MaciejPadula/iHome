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
        int GetRoomsCount(string uuid);
        List<Device> GetDevices(int roomId);
        bool AddDevice(int id, string deviceId, string deviceName, int deviceType, string deviceData, int roomId);
        bool RenameDevice(string deviceId, string deviceName);
        int GetDevicesCount(string uuid);
        string GetDeviceData(string deviceId);
        bool SetDeviceData(string deviceId, string deviceData);
        bool SetDeviceRoom(string deviceId, int roomId);
        List<TDeviceToConfigure>? GetDevicesToConfigure(string ip);
        bool AddDevicesToConfigure(string deviceId, int deviceType, string ip);
    }
}
