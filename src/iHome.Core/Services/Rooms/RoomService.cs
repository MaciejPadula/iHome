using iHome.Core.Exceptions;
using iHome.Core.Models;
using iHome.Core.Repositories;

namespace iHome.Core.Services.Rooms;
internal class RoomService : IRoomService
{
    private readonly SqlDataContext _infraDataContext;

    public RoomService(SqlDataContext infraDataContext)
    {
        _infraDataContext = infraDataContext;
    }

    public void AddRoom(string roomName, string userId)
    {
        if (_infraDataContext.Rooms
            .Any(room => room.Name == roomName && room.UserId == userId))
        {
            throw new RoomAlreadyExistsException();
        }
            
        _infraDataContext.Rooms.Add(new Room
        {
            Name = roomName,
            UserId = userId
        });
        _infraDataContext.SaveChanges();
    }

    public IEnumerable<Room> GetRooms(string userId)
    {
        return _infraDataContext.GetUsersRooms(userId);
    }

    public void RemoveRoom(Guid roomId, string userId)
    {
        var room = _infraDataContext.Rooms.FirstOrDefault(r => r.Id == roomId && r.UserId == userId);
        if (room == null) throw new RoomNotFoundException();

        _infraDataContext.Rooms.Remove(room);
        _infraDataContext.SaveChanges();
    }

    public void ShareRoom(Guid roomId, string userId)
    {
        if(!_infraDataContext.Rooms.Any(r => r.Id == roomId))
        {
            throw new RoomNotFoundException();
        }

        if (_infraDataContext.UserRoom
            .Any(share => share.RoomId == roomId || share.UserId == userId))
        {
            throw new RoomAlreadySharedException();
        }

        _infraDataContext.UserRoom.Add(new UserRoom
        {
            UserId = userId,
            RoomId = roomId
        });
        _infraDataContext.SaveChanges();
    }

    public void UnshareRoom(Guid roomId, string userId)
    {
        var constraint = _infraDataContext.UserRoom
            .Where(c => c.UserId == userId && c.RoomId == roomId)
            .SingleOrDefault();

        if(constraint == null) return;

        _infraDataContext.UserRoom.Remove(constraint);
        _infraDataContext.SaveChanges();
    }

    public bool UserCanAccessRoom(Guid roomId, string userId)
    {
        return _infraDataContext.Rooms.Where(room => room.Id == roomId).Any(room => room.UserId == userId) ||
            _infraDataContext.UserRoom.Where(room => room.RoomId == roomId).Any(room => room.UserId == userId);
    }
}
