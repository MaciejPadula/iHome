using iHome.Core.Logic.Providers;
using iHome.Core.Models;

namespace iHome.Core.Logic.AccessGuards;

public class EFDeviceAccessGuard : IDeviceAccessGuard
{
    private readonly IDeviceProvider _deviceProvider;
    private readonly IRoomAccessGuard _roomAccessGuard;

    public EFDeviceAccessGuard(IDeviceProvider deviceProvider, IRoomAccessGuard roomAccessGuard)
    {
        _deviceProvider = deviceProvider;
        _roomAccessGuard = roomAccessGuard;
    }

    public Task<bool> UserHasReadAccess(DeviceModel device, string userId)
    {
        return _roomAccessGuard.UserHasReadAccess(device.RoomId, userId);
    }

    public async Task<bool> UserHasReadAccess(Guid deviceId, string userId)
    {
        var device = await _deviceProvider.Get(deviceId);

        if (device == null) return false;

        return await UserHasReadAccess(device, userId);
    }

    public Task<bool> UserHasWriteAccess(DeviceModel device, string userId)
    {
        return _roomAccessGuard.UserHasWriteAccess(device.RoomId, userId);
    }

    public async Task<bool> UserHasWriteAccess(Guid deviceId, string userId)
    {
        var device = await _deviceProvider.Get(deviceId);

        if (device == null) return false;

        return await UserHasWriteAccess(device, userId);
    }
}
