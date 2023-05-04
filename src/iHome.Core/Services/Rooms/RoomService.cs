using iHome.Core.Exceptions.SqlExceptions;
using iHome.Core.Models;
using iHome.Core.Services.Users;
using iHome.Infrastructure.SQL.Contexts;
using iHome.Infrastructure.SQL.Models.ConnectionTables;
using iHome.Infrastructure.SQL.Models.RootTables;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Services.Rooms;

internal class RoomService : IRoomService
{
    private readonly IUserService _userService;
    private readonly SqlDataContext _sqlDataContext;

    public RoomService(IUserService userService, SqlDataContext sqlDataContext)
    {
        _userService = userService;
        _sqlDataContext = sqlDataContext;
    }

    public async Task AddRoom(string roomName, string userId)
    {
        if (await _sqlDataContext.Rooms
            .AnyAsync(room => room.Name == roomName && room.UserId == userId))
        {
            throw new RoomAlreadyExistsException();
        }

        await _sqlDataContext.Rooms.AddAsync(new Room
        {
            Name = roomName,
            UserId = userId
        });
        await _sqlDataContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<RoomModel>> GetRooms(string userId)
    {
        var rooms = await QueryRooms(userId)
            .ToListAsync();

        var roomModels = new List<RoomModel>();

        foreach (var room in rooms)
        {
            roomModels.Add(new RoomModel(room)
            {
                User = await _userService.GetUserById(userId) ?? new User { Id = room.UserId, Name = string.Empty, Email = string.Empty }
            });
        }

        return roomModels;
    }

    public async Task RemoveRoom(Guid roomId, string userId)
    {
        var room = await _sqlDataContext.Rooms.FirstOrDefaultAsync(r => r.Id == roomId && r.UserId == userId) ?? throw new RoomNotFoundException();

        _sqlDataContext.Rooms.Remove(room);
        await _sqlDataContext.SaveChangesAsync();
    }

    public async Task ShareRoom(Guid roomId, string userId, string callerUserId)
    {
        if (!await _userService.UserExist(new UserFilter { Id = userId })) throw new UserNotFoundException();

        if (!await _sqlDataContext.Rooms.AnyAsync(r => r.Id == roomId && r.UserId == callerUserId))
        {
            throw new RoomNotFoundException();
        }

        if (callerUserId == userId ||
            await _sqlDataContext.UserRoom
                .AnyAsync(share => share.RoomId == roomId && share.UserId == userId))
        {
            throw new RoomAlreadySharedException();
        }

        await _sqlDataContext.UserRoom.AddAsync(new UserRoom
        {
            UserId = userId,
            RoomId = roomId
        });
        await _sqlDataContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> GetRoomUsers(Guid roomId, string userId)
    {
        var room = await QueryRoom(roomId, userId) ?? throw new RoomNotFoundException();

        var users = new List<User>();

        foreach (var userRoom in room.UsersRooms)
        {
            var response = await _userService.GetUserById(userRoom.UserId);
            if (response == null) continue;

            users.Add(response);
        }

        return users;
    }

    public async Task UnshareRoom(Guid roomId, string userId, string callerUserId)
    {
        var room = await QueryRoom(roomId, callerUserId) ?? throw new RoomNotFoundException();

        var constraint = room.UsersRooms?
            .Where(c => c.UserId == userId && c.RoomId == roomId)
            .SingleOrDefault();

        if (constraint == null) return;

        _sqlDataContext.UserRoom.Remove(constraint);
        await _sqlDataContext.SaveChangesAsync();
    }

    public async Task<bool> UserCanAccessRoom(Guid roomId, string userId)
    {
        return await _sqlDataContext.Rooms.Where(room => room.Id == roomId).AnyAsync(room => room.UserId == userId) ||
            await _sqlDataContext.UserRoom.Where(room => room.RoomId == roomId).AnyAsync(room => room.UserId == userId);
    }

    private async Task<Room?> QueryRoom(Guid roomId, string userId)
    {
        return await QueryRooms(userId).Where(r => r.Id == roomId).FirstOrDefaultAsync();
    }

    private IQueryable<Room> QueryRooms(string userId)
    {
        return _sqlDataContext.Rooms
            .Include(r => r.UsersRooms)
            .Where(r => r.UserId == userId || r.UsersRooms.Any(u => u.UserId == userId));
    }
}
