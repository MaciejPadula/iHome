using iHome.Core.Exceptions.SqlExceptions;
using iHome.Core.Logic.AccessGuards;
using iHome.Microservices.UsersApi.Contract;
using iHome.Microservices.UsersApi.Contract.Models;

namespace iHome.Core.Logic.ActionValidators;

public interface IRoomActionValidator
{
    bool ValidateReadAccess(Guid roomId, string userId, out Exception ex);
    void ValidateReadAccessAndThrow(Guid roomId, string userId);

    bool ValidateWriteAccess(Guid roomId, string userId, out Exception ex);
    void ValidateWriteAccessAndThrow(Guid roomId, string userId);

    bool ValidateShareRoom(Guid roomId, string userId, string callerUserId, out Exception ex);
    void ValidateShareRoomAndThrow(Guid roomId, string userId, string callerUserId);
}

public class RoomActionValidator : IRoomActionValidator
{
    private readonly IRoomAccessGuard _roomAccessGuard;
    private readonly IUserManagementService _userService;

    public RoomActionValidator(IRoomAccessGuard roomAccessGuard, IUserManagementService userService)
    {
        _roomAccessGuard = roomAccessGuard;
        _userService = userService;
    }

    public bool ValidateReadAccess(Guid roomId, string userId, out Exception ex)
    {
        var result = ValidateAccess(out var exception, () => _roomAccessGuard.UserHasReadAccess(roomId, userId).Result);

        ex = exception;
        return result;
    }

    public void ValidateReadAccessAndThrow(Guid roomId, string userId)
    {
        if (!ValidateReadAccess(roomId, userId, out var exception)) throw exception;
    }

    public bool ValidateShareRoom(Guid roomId, string userId, string callerUserId, out Exception ex)
    {
        if (callerUserId == userId)
        {
            ex = new Exception();
            return false;
        }
        if (!_userService.UserExist(new() { Filter = new UserFilter { Id = userId } }).Result.Exists)
        {
            ex = new UserNotFoundException();
            return false;
        }
        if (!_roomAccessGuard.UserHasWriteAccess(roomId, callerUserId).Result)
        {
            ex = new RoomNotFoundException();
            return false;
        }
        if (_roomAccessGuard.RoomAlreadyShared(roomId, userId).Result)
        {
            ex = new RoomAlreadySharedException();
            return false;
        }

        ex = default!;
        return true;
    }

    public void ValidateShareRoomAndThrow(Guid roomId, string userId, string callerUserId)
    {
        if (!ValidateShareRoom(roomId, userId, callerUserId, out var exception)) throw exception;
    }

    public bool ValidateWriteAccess(Guid roomId, string userId, out Exception ex)
    {
        var result = ValidateAccess(out var exception, () => _roomAccessGuard.UserHasWriteAccess(roomId, userId).Result);

        ex = exception;
        return result;
    }

    public void ValidateWriteAccessAndThrow(Guid roomId, string userId)
    {
        if (!ValidateWriteAccess(roomId, userId, out var exception)) throw exception;
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
