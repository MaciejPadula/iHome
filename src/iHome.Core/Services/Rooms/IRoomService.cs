using iHome.Core.Models;

namespace iHome.Core.Services.Rooms;
public interface IRoomService
{
    void AddRoom(string roomName, Guid userId);
    IEnumerable<Room> GetRooms(Guid userId);
    void RemoveRoom(Guid userId, Guid roomId);

    void ShareRoom(Guid userId, Guid roomId);
    void UnshareRoom(Guid userId, Guid roomId);
}
