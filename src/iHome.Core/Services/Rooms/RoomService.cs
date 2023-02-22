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

        if (_sqlDataContext.UserRoom
            .Any(share => share.RoomId == roomId || share.UserId == userId))
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
        var room = QueryRooms(userId)
            .Where(room => room.Id == roomId)
            .Include(r => r.UsersRooms)
            .FirstOrDefault();

        if (room == null) throw new RoomNotFoundException();

        return room.UsersRooms;
    }

    public void UnshareRoom(Guid roomId, string userId, string callerUserId)
    {
        var room = QueryRooms(callerUserId)
            .Include(r => r.UsersRooms)
            .FirstOrDefault(r => r.Id == roomId);

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

    private IQueryable<Room> QueryRooms(string userId)
    {
        return _sqlDataContext.Rooms
            .Where(r => r.UserId == userId)
            .GroupJoin(
                _sqlDataContext.UserRoom,
                room => room.Id,
                userRoom => userRoom.RoomId,
                (room, userRoom) => room
            );

        //var sharedRooms = _sqlDataContext.UserRoom
        //    .Where(r => r.UserId == userId)
        //    .Include(r => r.Room)
        //    .Select(r => r.Room)
        //    .OfType<Room>();

        //return rooms.Union(sharedRooms);
    }
}
