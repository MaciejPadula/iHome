using iHome.Microservices.Devices.Contract.Models;

namespace iHome.Microservices.Devices.Domain.Repositories;

public interface IDeviceRepository
{
    Task<Guid> Add(string name, string macAddress, DeviceType type, Guid roomId);
    Task<IEnumerable<DeviceModel>> GetByRoomId(Guid roomId);
    Task<DeviceModel?> GetByDeviceId(Guid deviceId);
    Task<IEnumerable<DeviceModel>> GetByUserIdAndDeviceIds(string userId, IEnumerable<Guid> deviceIds);
    Task<IEnumerable<DeviceModel>> GetByUserId(string userId);
    Task Remove(Guid deviceId);
    Task Rename(Guid deviceId, string name);
    Task ChangeRoom(Guid deviceId, Guid roomId);
}
