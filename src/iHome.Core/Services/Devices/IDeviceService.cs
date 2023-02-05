using iHome.Devices.Contract.Models;

namespace iHome.Core.Services.Devices;

public interface IDeviceService
{
    Guid AddDevice(string name, string macAddress, DeviceType type, string hubId, Guid roomId, Guid userId);
    Device GetDevice(Guid deviceId, Guid userId);
    IEnumerable<Device> GetDevices(Guid userId);
    void RemoveDevice(Guid deviceId, Guid userId);
    void RenameDevice(Guid deviceId, string newName, Guid userId);
    void ChangeDeviceRoom(Guid deviceId, Guid roomId, Guid userId);
    void SetDeviceData(Guid deviceId, string data, Guid userId);
}
