using iHome.Infrastructure.SQL.Contexts;
using iHome.Infrastructure.SQL.Models.ConnectionTables;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Repositories.Rooms;

public class EFUserRoomRepository : IUserRoomRepository
{
    private readonly SqlDataContext _sqlDataContext;

    public EFUserRoomRepository(SqlDataContext sqlDataContext)
    {
        _sqlDataContext = sqlDataContext;
    }

    public async Task AddUserRoom(Guid roomId, string userId)
    {
        await _sqlDataContext.UserRoom.AddAsync(new UserRoom
        {
            UserId = userId,
            RoomId = roomId
        });
        await _sqlDataContext.SaveChangesAsync();
    }

    public Task<List<string>> GetRoomUsersIds(Guid roomId)
    {
        return _sqlDataContext.UserRoom
            .Where(r => r.RoomId == roomId)
            .Select(r => r.UserId)
            .ToListAsync();
    }

    public async Task RemoveUserRoom(Guid roomId, string userId)
    {
        var constraint = await _sqlDataContext.UserRoom
            .Where(c => c.UserId == userId && c.RoomId == roomId)
            .SingleOrDefaultAsync();

        if (constraint == null) return;

        _sqlDataContext.UserRoom.Remove(constraint);
        await _sqlDataContext.SaveChangesAsync();
    }
}
