using iHome.Infrastructure.SQL.Contexts;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Logic.AccessGuards;

public class EFRoomAccessGuard : IRoomAccessGuard
{
    private readonly SqlDataContext _sqlDataContext;

    public EFRoomAccessGuard(SqlDataContext sqlDataContext)
    {
        _sqlDataContext = sqlDataContext;
    }

    public Task<bool> RoomAlreadyExists(string name, string userId)
    {
        return _sqlDataContext.Rooms
            .AnyAsync(room => room.Name == name && room.UserId == userId);
    }

    public Task<bool> RoomAlreadyShared(Guid roomId, string userId)
    {
        return _sqlDataContext.UserRoom
                .AnyAsync(share => share.RoomId == roomId && share.UserId == userId);
    }

    public async Task<bool> UserHasReadAccess(Guid roomId, string userId)
    {
        return await UserHasWriteAccess(roomId, userId) ||
            await _sqlDataContext.UserRoom.Where(room => room.RoomId == roomId && room.UserId == userId).AnyAsync();
    }

    public Task<bool> UserHasWriteAccess(Guid roomId, string userId)
    {
        return _sqlDataContext.Rooms.Where(room => room.Id == roomId && room.UserId == userId).AnyAsync();
    }
}
