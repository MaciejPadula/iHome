using iHome.Core.Exceptions;
using iHome.Core.Models;
using iHome.Core.Repositories;

namespace iHome.Core.Services.Rooms;
internal class RoomService : IRoomService
{
    private readonly IRepository _repository;

    public RoomService(IRepository repository)
    {
        _repository = repository;
    }

    public void AddRoom(string roomName, Guid userId)
    {
        if (_repository.Rooms
            .Any(room => room.Name == roomName && room.UserId == userId))
        {
            throw new RoomAlreadyExistsException();
        }
            
        _repository.Rooms.Add(new Room
        {
            Name = roomName,
            UserId = userId
        });
        _repository.SaveChanges();
    }

    public IEnumerable<Room> GetRooms(Guid userId)
    {
        return _repository.Rooms
            .Where(r => r.UserId == userId || UserCanUseRoom(r.Id, userId))
            .ToList();
    }

    private bool UserCanUseRoom(Guid roomId, Guid userId)
    {
        return _repository.SharedRooms
            .Where(s => s.UserId == userId && s.RoomId == roomId)
            .Any();
    }

    public void RemoveRoom(Guid roomId, Guid userId)
    {
        var room = _repository.Rooms.FirstOrDefault(r => r.Id == roomId && r.UserId == userId);
        if (room == null) throw new RoomNotFoundException();

        _repository.Rooms.Remove(room);
        _repository.SaveChanges();
    }

    public void ShareRoom(Guid roomId, Guid userId)
    {
        if(!_repository.Rooms.Any(r => r.Id == roomId))
        {
            throw new RoomNotFoundException();
        }

        if (_repository.SharedRooms
            .Any(share => share.RoomId == roomId || share.UserId == userId))
        {
            throw new RoomAlreadySharedException();
        }

        _repository.SharedRooms.Add(new SharedRoom
        {
            UserId = userId,
            RoomId = roomId
        });
        _repository.SaveChanges();
    }

    public void UnshareRoom(Guid roomId, Guid userId)
    {
        var constraint = _repository.SharedRooms
            .Where(c => c.UserId == userId && c.RoomId == roomId)
            .SingleOrDefault();

        if(constraint == null) return;

        _repository.SharedRooms.Remove(constraint);
        _repository.SaveChanges();
    }
}
