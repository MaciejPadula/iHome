using iHome.Core.Models;

namespace iHome.Core.Services.Rooms;
public interface IRoomService
{
    bool UserCanAccessRoom(Guid roomId, Guid userId);

    void AddRoom(string roomName, Guid userId);
    IEnumerable<Room> GetRooms(Guid userId);
    void RemoveRoom(Guid roomId, Guid userId);

    void ShareRoom(Guid roomId, Guid userId);
    void UnshareRoom(Guid roomId, Guid userId);
}
