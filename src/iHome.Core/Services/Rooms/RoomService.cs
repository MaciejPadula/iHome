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
        if (_repository.Rooms.Any(room => room.Name == roomName && room.UserId == userId))
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
            .Where(r => r.UserId == userId)
            .ToList();
    }

    public void RemoveRoom(Guid userId, Guid roomId)
    {
        var room = _repository.Rooms.FirstOrDefault(r => r.Id == roomId && r.UserId == userId);
        if (room == null) return;

        _repository.Rooms.Remove(room);
        _repository.SaveChanges();
    }

    public void ShareRoom(Guid userId, Guid roomId)
    {
        var room = _repository.Rooms.FirstOrDefault(r => r.Id == roomId);
        if(room == null) return;

        _repository.SharedRooms.Add(new SharedRoom
        {
            UserId = userId,
            RoomId = roomId
        });
        _repository.SaveChanges();
    }

    public void UnshareRoom(Guid userId, Guid roomId)
    {
        var constraint = _repository.SharedRooms
            .Where(c => c.UserId == userId && c.RoomId == roomId)
            .SingleOrDefault();

        if(constraint == null) return;

        _repository.SharedRooms.Remove(constraint);
        _repository.SaveChanges();
    }
}
