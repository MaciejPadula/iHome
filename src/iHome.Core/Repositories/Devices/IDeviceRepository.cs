using iHome.Core.Models;
using iHome.Infrastructure.SQL.Models.Enums;

namespace iHome.Core.Repositories.Devices;

public interface IDeviceRepository
{
    Task<Guid> Add(string name, string macAddress, DeviceType type, Guid roomId);
    Task<List<DeviceModel>> GetByRoomId(Guid roomId);
    Task<List<DeviceModel>> GetByDeviceId(Guid deviceId);
    Task<List<DeviceModel>> GetByUserId(string userId);
    Task Remove(Guid deviceId);
    Task Rename(Guid deviceId, string name);
    Task ChangeRoom(Guid deviceId, Guid roomId);
}
