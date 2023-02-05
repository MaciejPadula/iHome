using iHome.Core.Exceptions;
using iHome.Core.Models;
using iHome.Core.Repositories;

namespace iHome.Core.Services.Rooms;
internal class RoomService : IRoomService
{
    private readonly InfraDataContext _infraDataContext;

    public RoomService(InfraDataContext infraDataContext)
    {
        _infraDataContext = infraDataContext;
    }

    public void AddRoom(string roomName, Guid userId)
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

    public IEnumerable<Room> GetRooms(Guid userId)
    {
        return _infraDataContext.Rooms
            .Where(room => room.UserId == userId)
            .GroupJoin(
                _infraDataContext.SharedRooms.Where(sharedRoom => sharedRoom.UserId == userId),
                room => room.Id,
                sharedRoom => sharedRoom.RoomId,
                (room, sharedRoom) => room
            );
    }

    public void RemoveRoom(Guid roomId, Guid userId)
    {
        var room = _infraDataContext.Rooms.FirstOrDefault(r => r.Id == roomId && r.UserId == userId);
        if (room == null) throw new RoomNotFoundException();

        _infraDataContext.Rooms.Remove(room);
        _infraDataContext.SaveChanges();
    }

    public void ShareRoom(Guid roomId, Guid userId)
    {
        if(!_infraDataContext.Rooms.Any(r => r.Id == roomId))
        {
            throw new RoomNotFoundException();
        }

        if (_infraDataContext.SharedRooms
            .Any(share => share.RoomId == roomId || share.UserId == userId))
        {
            throw new RoomAlreadySharedException();
        }

        _infraDataContext.SharedRooms.Add(new SharedRoom
        {
            UserId = userId,
            RoomId = roomId
        });
        _infraDataContext.SaveChanges();
    }

    public void UnshareRoom(Guid roomId, Guid userId)
    {
        var constraint = _infraDataContext.SharedRooms
            .Where(c => c.UserId == userId && c.RoomId == roomId)
            .SingleOrDefault();

        if(constraint == null) return;

        _infraDataContext.SharedRooms.Remove(constraint);
        _infraDataContext.SaveChanges();
    }

    public bool UserCanAccessRoom(Guid roomId, Guid userId)
    {
        return _infraDataContext.Rooms.Any(room => room.UserId == userId) ||
            _infraDataContext.SharedRooms.Any(room => room.UserId == userId);
    }
}
