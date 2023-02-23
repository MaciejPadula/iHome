using iHome.Core.Exceptions;
using iHome.Core.Models;
using iHome.Core.Repositories;
using iHome.Core.Services.Users;
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

    public void AddRoom(string roomName, string userId)
    {
        if (_sqlDataContext.Rooms
            .Any(room => room.Name == roomName && room.UserId == userId))
        {
            throw new RoomAlreadyExistsException();
        }
            
        _sqlDataContext.Rooms.Add(new Room
        {
            Name = roomName,
            UserId = userId
        });
        _sqlDataContext.SaveChanges();
    }

    public IEnumerable<Room> GetRooms(string userId)
    {
        return QueryRooms(userId);
    }

    public IEnumerable<Room> GetRoomsWithDevices(string userId)
    {
        return QueryRooms(userId)
            .Include(r => r.Devices);
    }

    public void RemoveRoom(Guid roomId, string userId)
    {
        var room = _sqlDataContext.Rooms.FirstOrDefault(r => r.Id == roomId && r.UserId == userId);
        if (room == null) throw new RoomNotFoundException();

        _sqlDataContext.Rooms.Remove(room);
        _sqlDataContext.SaveChanges();
    }

    public void ShareRoom(Guid roomId, string userId, string callerUserId)
    {
        if (!_userService.UserExist(new UserFilter { Id = userId })) throw new UserNotFoundException();

        if (!_sqlDataContext.Rooms.Any(r => r.Id == roomId && r.UserId == callerUserId))
        {
            throw new RoomNotFoundException();
        }

        if (callerUserId == userId ||
            _sqlDataContext.UserRoom
                .Any(share => share.RoomId == roomId && share.UserId == userId))
        {
            throw new RoomAlreadySharedException();
        }

        _sqlDataContext.UserRoom.Add(new UserRoom
        {
            UserId = userId,
            RoomId = roomId
        });
        _sqlDataContext.SaveChanges();
    }

    public IEnumerable<UserRoom> GetRoomUsers(Guid roomId, string userId)
    {
        var room = QueryRoom(roomId, userId);

        if (room == null) throw new RoomNotFoundException();

        return room.UsersRooms;
    }

    public void UnshareRoom(Guid roomId, string userId, string callerUserId)
    {
        var room = QueryRoom(roomId, callerUserId);

        if (room == null) throw new RoomNotFoundException();

        var constraint = room.UsersRooms?
            .Where(c => c.UserId == userId && c.RoomId == roomId)
            .SingleOrDefault();

        if(constraint == null) return;

        _sqlDataContext.UserRoom.Remove(constraint);
        _sqlDataContext.SaveChanges();
    }

    public bool UserCanAccessRoom(Guid roomId, string userId)
    {
        return _sqlDataContext.Rooms.Where(room => room.Id == roomId).Any(room => room.UserId == userId) ||
            _sqlDataContext.UserRoom.Where(room => room.RoomId == roomId).Any(room => room.UserId == userId);
    }

    private Room? QueryRoom(Guid roomId, string userId)
    {
        return QueryRooms(userId).FirstOrDefault(r => r.Id == roomId);
    }

    private IQueryable<Room> QueryRooms(string userId)
    {
        return _sqlDataContext.Rooms
            .Include(r => r.UsersRooms)
            .Where(r => r.UserId == userId || r.UsersRooms.Any(u => u.UserId == userId));
    }
}
