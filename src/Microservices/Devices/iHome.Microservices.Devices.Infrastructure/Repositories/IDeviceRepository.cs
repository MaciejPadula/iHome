using iHome.Microservices.Devices.Contract.Models;

namespace iHome.Core.Repositories.Devices;

public interface IDeviceRepository
{
    Task<Guid> Add(string name, string macAddress, DeviceType type, Guid roomId);
    Task<IEnumerable<DeviceModel>> GetByRoomId(Guid roomId);
    Task<DeviceModel?> GetByDeviceId(Guid deviceId);
    Task<IEnumerable<DeviceModel>> GetByUserId(string userId);
    Task Remove(Guid deviceId);
    Task Rename(Guid deviceId, string name);
    Task ChangeRoom(Guid deviceId, Guid roomId);
}
