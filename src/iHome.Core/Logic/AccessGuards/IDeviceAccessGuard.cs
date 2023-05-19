using iHome.Core.Models;

namespace iHome.Core.Logic.AccessGuards;

public interface IDeviceAccessGuard
{
    Task<bool> UserHasReadAccess(DeviceModel device, string userId);
    Task<bool> UserHasWriteAccess(DeviceModel device, string userId);

    Task<bool> UserHasReadAccess(Guid deviceId, string userId);
    Task<bool> UserHasWriteAccess(Guid deviceId, string userId);
}
