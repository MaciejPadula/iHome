using iHome.Core.Models.ApiRooms;
using iHome.Core.Models.Database;

namespace iHome.Core.Services.DatabaseService
{
    public interface IDatabaseService
    {
        Task AddRoom(string roomName, string roomDescription, string uuid);
        Task<List<Room>> GetListOfRooms(string uuid);
        Task RemoveRoom(int roomId);

        Task ShareRoom(int roomId, string uuid);
        Task RemoveRoomShare(int roomId, string uuid, string masterUuid);

        Task AddDevice(int id, string deviceId, string deviceName, int deviceType, string deviceData, int roomId);        
        Task<List<Device>> GetDevices(int roomId);
        Task RenameDevice(string deviceId, string deviceName, string uuid);
        Task SetDeviceData(string deviceId, string deviceData, string uuid);
        Task<string> GetDeviceData(string deviceId, string uuid);
        
        Task SetDeviceRoom(string deviceId, int roomId, string uuid);
        Task<List<TDeviceToConfigure>> GetDevicesToConfigure(string ip);
        Task<List<string>> GetRoomUserIds(int roomId);
        Task<int> GetDeviceRoomId(string deviceId);
        
    }
}
