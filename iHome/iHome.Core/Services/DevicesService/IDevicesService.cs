using iHome.Core.Models.ApiRooms;
using iHome.Core.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHome.Core.Services.DevicesService
{
    public interface IDevicesService
    {
        Task AddDevice(int id, string deviceId, string deviceName, int deviceType, string deviceData, int roomId);

        Task<string> GetDeviceData(string deviceId, string uuid);
        Task SetDeviceData(string deviceId, string deviceData, string uuid);

        Task RenameDevice(string deviceId, string deviceName, string uuid);
        Task<int> GetDeviceRoomId(string deviceId);
        Task SetDeviceRoom(string deviceId, int roomId, string uuid);
        
        Task<List<TDeviceToConfigure>> GetDevicesToConfigure(string ip);
    }
}
