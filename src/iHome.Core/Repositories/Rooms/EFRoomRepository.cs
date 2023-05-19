using iHome.Core.Exceptions.SqlExceptions;
using iHome.Core.Models;
using iHome.Infrastructure.SQL.Contexts;
using iHome.Infrastructure.SQL.Models.RootTables;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Repositories.Rooms;

public class EFRoomRepository : IRoomRepository
{
    private readonly SqlDataContext _sqlDataContext;

    public EFRoomRepository(SqlDataContext sqlDataContext)
    {
        _sqlDataContext = sqlDataContext;
    }

    public async Task Add(string name, string userId)
    {
        await _sqlDataContext.Rooms.AddAsync(new Room
        {
            Id = Guid.NewGuid(),
            Name = name,
            UserId = userId,
        });

        await _sqlDataContext.SaveChangesAsync();
    }

    public Task<List<RoomModel>> GetById(Guid roomId)
    {
        return _sqlDataContext.Rooms.Where(r => r.Id == roomId).Select(r => new RoomModel(r))
            .ToListAsync();
    }

    public Task<List<RoomModel>> GetByRoomIdAndUserId(Guid roomId, string userId)
    {
        return _sqlDataContext.Rooms.Where(r => r.Id == roomId && r.UserId == userId).Select(r => new RoomModel(r))
            .ToListAsync();
    }

    public async Task<List<RoomModel>> GetByUserId(string userId)
    {
        var ownedRooms = await _sqlDataContext.Rooms
            .Where(r => r.UserId == userId)
            .Select(r => new RoomModel(r))
            .ToListAsync();

        var sharedRooms = await _sqlDataContext.UserRoom
            .Where(r => r.UserId == userId)
            .Include(r => r.Room)
            .Select(r => r.Room)
            .OfType<Room>()
            .Select(r => new RoomModel(r))
            .ToListAsync();

        return ownedRooms.Union(sharedRooms).ToList();
    }

    public Task Remove(Guid roomId)
    {
        return UpdateRoom(roomId, (ctx, room) => ctx.Remove(room));
    }

    public Task Update(Guid roomId, string name)
    {
        return UpdateRoom(roomId, (ctx, room) => room.Name = name);
    }

    private async Task UpdateRoom(Guid roomId, Action<SqlDataContext, Room> updater)
    {
        var room = await _sqlDataContext.Rooms
            .FirstOrDefaultAsync(d => d.Id == roomId) ?? throw new RoomNotFoundException();

        updater.Invoke(_sqlDataContext, room);

        await _sqlDataContext.SaveChangesAsync();
    }
}
