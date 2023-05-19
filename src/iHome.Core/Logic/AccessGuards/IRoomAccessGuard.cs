namespace iHome.Core.Logic.AccessGuards;

public interface IRoomAccessGuard
{
    Task<bool> UserHasWriteAccess(Guid roomId, string userId);
    Task<bool> UserHasReadAccess(Guid roomId, string userId);

    Task<bool> RoomAlreadyExists(string name, string userId);
    Task<bool> RoomAlreadyShared(Guid roomId, string userId);
}
