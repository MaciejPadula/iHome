using iHome.Core.Exceptions;
using iHome.Core.Models;
using iHome.Core.Repositories;

namespace iHome.Core.Services.Rooms;
internal class RoomService : IRoomService
{
    private readonly SqlDataContext _sqlDataContext;

    public RoomService(SqlDataContext sqlDataContext)
    {
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
        return _sqlDataContext.GetUsersRooms(userId);
    }

    public void RemoveRoom(Guid roomId, string userId)
    {
        var room = _sqlDataContext.Rooms.FirstOrDefault(r => r.Id == roomId && r.UserId == userId);
        if (room == null) throw new RoomNotFoundException();

        _sqlDataContext.Rooms.Remove(room);
        _sqlDataContext.SaveChanges();
    }

    public void ShareRoom(Guid roomId, string userId)
    {
        if(!_sqlDataContext.Rooms.Any(r => r.Id == roomId))
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

    public void UnshareRoom(Guid roomId, string userId)
    {
        var constraint = _sqlDataContext.UserRoom
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
}
