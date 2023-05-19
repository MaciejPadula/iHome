using iHome.Core.Exceptions.SqlExceptions;
using iHome.Core.Logic.AccessGuards;
using iHome.Core.Models;

namespace iHome.Core.Logic.ActionValidators;

public interface IDeviceActionValidator
{
    bool ValidateAdd(Guid roomId, string macAddress, string userId, out Exception ex);
    void ValidateAddAndThrow(Guid roomId, string macAddress, string userId);

    bool ValidateGet(Guid roomId, string userId, out Exception ex);
    void ValidateGetAndThrow(Guid roomId, string userId);

    bool ValidateRemove(DeviceModel device, string userId, out Exception ex);
    void ValidateRemoveAndThrow(DeviceModel device, string userId);
}

public class DeviceActionValidator : IDeviceActionValidator
{
    private readonly IRoomAccessGuard _roomAccessGuard;
    private readonly IDeviceAccessGuard _deviceAccessGuard;

    public DeviceActionValidator(IRoomAccessGuard roomAccessGuard, IDeviceAccessGuard deviceAccessGuard)
    {
        _roomAccessGuard = roomAccessGuard;
        _deviceAccessGuard = deviceAccessGuard;
    }

    public bool ValidateAdd(Guid roomId, string macAddress, string userId, out Exception ex)
    {
        var result = ValidateAccess(out var exception, () => _roomAccessGuard.UserHasWriteAccess(roomId, userId).Result);

        ex = exception;
        return result;
    }

    public void ValidateAddAndThrow(Guid roomId, string macAddress, string userId)
    {
        if (!ValidateAdd(roomId, macAddress, userId, out var ex)) throw ex;
    }

    public bool ValidateGet(Guid roomId, string userId, out Exception ex)
    {
        var result = ValidateAccess(out var exception, () => _roomAccessGuard.UserHasReadAccess(roomId, userId).Result);

        ex = exception;
        return result;
    }

    public void ValidateGetAndThrow(Guid roomId, string userId)
    {
        if (!ValidateGet(roomId, userId, out var ex)) throw ex;
    }

    public bool ValidateRemove(DeviceModel device, string userId, out Exception ex)
    {
        if (!_deviceAccessGuard.UserHasWriteAccess(device, userId).Result)
        {
            ex = new DeviceNotFoundException();
            return false;
        }

        ex = default!;
        return true;
    }

    public void ValidateRemoveAndThrow(DeviceModel device, string userId)
    {
        if (!ValidateRemove(device, userId, out var ex)) throw ex;
    }

    private bool ValidateAccess(out Exception ex, Func<bool> checker)
    {
        if (!checker.Invoke())
        {
            ex = new AccessDeniedException();
            return false;
        }

        ex = default!;
        return true;
    }
}
