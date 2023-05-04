using iHome.Core.Models;
using iHome.Infrastructure.SQL.Models.Enums;

namespace iHome.Core.Services.Devices;

public interface IDeviceService
{
    Task<Guid> AddDevice(string name, string macAddress, DeviceType type, Guid roomId, string userId);
    Task<DeviceModel> GetDevice(Guid deviceId, string userId);
    Task<bool> DeviceExists(Guid deviceId, string userId);
    Task<IEnumerable<DeviceModel>> GetDevices(Guid roomId, string userId);
    Task<IEnumerable<DeviceModel>> GetDevices(string userId);
    Task<IEnumerable<DeviceModel>> GetDevicesForScheduling(string userId);
    Task RemoveDevice(Guid deviceId, string userId);
    Task RenameDevice(Guid deviceId, string newName, string userId);
    Task ChangeDeviceRoom(Guid deviceId, Guid roomId, string userId);
    Task SetDeviceData(Guid deviceId, string data, string userId);
    Task<string> GetDeviceData(Guid deviceId, string userId);
}
