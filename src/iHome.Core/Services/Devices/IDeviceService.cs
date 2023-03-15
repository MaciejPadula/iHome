using iHome.Core.Models;
using iHome.Devices.Contract.Models;

namespace iHome.Core.Services.Devices;

public interface IDeviceService
{
    Guid AddDevice(string name, string macAddress, DeviceType type, Guid roomId, string userId);
    Device GetDevice(Guid deviceId, string userId);
    IEnumerable<Device> GetDevices(Guid roomId, string userId);
    void RemoveDevice(Guid deviceId, string userId);
    void RenameDevice(Guid deviceId, string newName, string userId);
    void ChangeDeviceRoom(Guid deviceId, Guid roomId, string userId);
    void SetDeviceData(Guid deviceId, string data, string userId);
    string GetDeviceData(Guid deviceId, string userId);
}
