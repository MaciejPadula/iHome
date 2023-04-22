using iHome.Infrastructure.SQL.Models;

namespace iHome.Core.Services.Devices;

public interface IDeviceService
{
    Task<Guid> AddDevice(string name, string macAddress, DeviceType type, Guid roomId, string userId);
    Task<Device> GetDevice(Guid deviceId, string userId);
    Task<IEnumerable<Device>> GetDevices(Guid roomId, string userId);
    Task RemoveDevice(Guid deviceId, string userId);
    Task RenameDevice(Guid deviceId, string newName, string userId);
    Task ChangeDeviceRoom(Guid deviceId, Guid roomId, string userId);
    Task SetDeviceData(Guid deviceId, string data, string userId);
    Task<string> GetDeviceData(Guid deviceId, string userId);
}
